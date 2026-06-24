Shader "Hidden/URP_Retro16Bit_PostProcess"
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

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // Этот инклюд автоматически предоставляет вершины и UV для полноэкранного блита
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _RedBits;
            float _GreenBits;
            float _BlueBits;
            float _PixelationActive;
            float _PixelSizeX;
            float _PixelSizeY;

            float QuantizeChannel(float rawColor, float bits)
            {
                float levels = pow(2.0, bits) - 1.0;
                return round(rawColor * levels) / levels;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                // В Blit.hlsl координаты экрана лежат в input.texcoord
                float2 uv = input.texcoord;

                // Пикселизация экрана
                if (_PixelationActive > 0.5)
                {
                    uv.x = round(uv.x * _PixelSizeX) / _PixelSizeX;
                    uv.y = round(uv.y * _PixelSizeY) / _PixelSizeY;
                }

                // Выборка пикселя из кадра камеры (_BlitTexture — стандартное имя в URP)
                float4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                // Поканальное уменьшение битности
                float3 retroColor;
                retroColor.r = QuantizeChannel(color.r, _RedBits);
                retroColor.g = QuantizeChannel(color.g, _GreenBits);
                retroColor.b = QuantizeChannel(color.b, _BlueBits);

                // Возвращаем результат. Альфа-канал экрана обычно равен 1.0, сохраняем его
                return float4(retroColor, color.a);
            }
            ENDHLSL
        }
    }
}