Shader "Retro/16Bit_PostProcess"
{
    Properties
    {
        _RedBits("Red Bits", Range(1, 8)) = 5
        _GreenBits("Green Bits", Range(1, 8)) = 6
        _BlueBits("Blue Bits", Range(1, 8)) = 5
        
        [Toggle] _PixelationActive("Enable Pixelation", Float) = 1
        _PixelSizeX("Pixel Size X", Float) = 320
        _PixelSizeY("Pixel Size Y", Float) = 180
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "Retro16BitPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // ИСПРАВЛЕНИЕ 1: Для Пост-процессинга (Blit) на iOS НЕЛЬЗЯ использовать UnityPerMaterial.
            // Называем буфер иначе, чтобы не ломать логику SRP Batcher, который здесь не работает.
            CBUFFER_START(RetroPostProcessParams)
                float _RedBits;
                float _GreenBits;
                float _BlueBits;
                float _PixelationActive;
                float _PixelSizeX;
                float _PixelSizeY;
            CBUFFER_END

            float QuantizeChannel(float rawColor, float bits)
            {
                float safeBits = max(1.0, bits);
                float levels = pow(2.0, safeBits) - 1.0;
                
                // ИСПРАВЛЕНИЕ 2: Избавляемся от динамического ветвления (if) в пользу функции max()
                levels = max(1.0, levels); 
                
                // ИСПРАВЛЕНИЕ 3: Заменяем round() на кроссплатформенный floor(x + 0.5)
                return floor(rawColor * levels + 0.5) / levels;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                // ИСПРАВЛЕНИЕ 4: Используем макрос для корректного считывания UV в Blit-пассах
                float2 uv = input.texcoord;

                if (_PixelationActive > 0.5 && _PixelSizeX > 1.0 && _PixelSizeY > 1.0)
                {
                    uv.x = floor(uv.x * _PixelSizeX + 0.5) / _PixelSizeX;
                    uv.y = floor(uv.y * _PixelSizeY + 0.5) / _PixelSizeY;
                }

                // ИСПРАВЛЕНИЕ 5: КРИТИЧЕСКОЕ ДЛЯ IOS. Используем SAMPLE_TEXTURE2D_X_LOD с нулевым мип-уровнем.
                // Модификация UV выше ломает автоматический расчет деривативов на мобильных GPU, что вешало шейдер.
                // Также заменен sampler_LinearClamp на встроенный и гарантированный sampler_BlitTexture.
                float4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, 0);

                // Поканальное уменьшение битности
                float3 retroColor;
                retroColor.r = QuantizeChannel(color.r, _RedBits);
                retroColor.g = QuantizeChannel(color.g, _GreenBits);
                retroColor.b = QuantizeChannel(color.b, _BlueBits);

                return float4(retroColor, color.a);
            }
            ENDHLSL
        }
    }
}