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
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // ОБЯЗАТЕЛЬНО ДЛЯ IOS: Упаковка свойств в константный буфер
            CBUFFER_START(UnityPerMaterial)
                float _RedBits;
                float _GreenBits;
                float _BlueBits;
                float _PixelationActive;
                float _PixelSizeX;
                float _PixelSizeY;
            CBUFFER_END

            float QuantizeChannel(float rawColor, float bits)
            {
                // Защита: гарантируем, что bits не упадет ниже 1.0
                float safeBits = max(1.0, bits);
                float levels = pow(2.0, safeBits) - 1.0;
                
                // Защита от деления на ноль
                if (levels < 1.0) levels = 1.0; 
                
                return round(rawColor * levels) / levels;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                // Пикселизация экрана с защитой от нулевого разрешения
                if (_PixelationActive > 0.5 && _PixelSizeX > 1.0 && _PixelSizeY > 1.0)
                {
                    uv.x = round(uv.x * _PixelSizeX) / _PixelSizeX;
                    uv.y = round(uv.y * _PixelSizeY) / _PixelSizeY;
                }

                // Выборка пикселя из кадра камеры
                float4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

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