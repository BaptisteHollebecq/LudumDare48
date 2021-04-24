Shader "Custom/PostRendering/Pencil"
{
    //Original Author: Daniel Taylor https://www.shadertoy.com/view/ldXfRj
    //License Creative Commons Attribution - NonCommercial - ShareAlike 3.0 Unported License.
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BackgroundTex("Background Texture", 2D) = "white" {}
        _GradThresh("Gradiant threshold", range(0.000001, 0.01)) = 0.01
        _ColorThreshold("Color Threshold", range(0.0, 1)) = 0.5
        _Samples("Samples", range(2,10)) = 2
        _Senstivity("Senstivity", range(0,20)) = 10
        _BackgroundInt("Background Int", range(0,1)) = 1

    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 4.0
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _BackgroundTex;

                float _GradThresh;
                float _ColorThreshold;
                float _Intensity;
                float _Samples;
                float _Senstivity;
                float _BackgroundInt;


                struct v2f {
                    float4 pos : SV_POSITION;
                    float4 screenuv : TEXCOORD0;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.screenuv = ComputeScreenPos(o.pos);
                    return o;
                }

                #define PI2 6.28318530717959
                #define STEP 2.0
                #define RANGE 16.0
                #define GRADTHRESH 0.01

                float4 getCol(float2 pos)
                {
                    return tex2D(_MainTex, pos / _ScreenParams.xy);
                }

                float getVal(float2 pos)
                {
                    float4 c = getCol(pos);
                    return dot(c.xyz, float3(0.2126, 0.7152, 0.0722));
                }

                float2 getGrad(float2 pos, float delta)
                {
                    float2 d = float2(delta, 0.0);
                    return float2(getVal(pos + d.xy) - getVal(pos - d.xy),
                                  getVal(pos + d.yx) - getVal(pos - d.yx)) / delta / 2.0;
                }

                void pR(inout float2 p, float a) {
                    p = cos(a) * p + sin(a) * float2(p.y, -p.x);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 screenuv = i.screenuv.xy / i.screenuv.w;
                    float2 screenPos = float2(i.screenuv.x * _ScreenParams.x, i.screenuv.y * _ScreenParams.y);
                    float weight = 1.0;

                    for (float j = 0; j < _Samples; j++)
                    {
                        float2 dir = float2(1.0, 0.0);
                        pR(dir, j * PI2 / (2.0 * _Samples));

                        float2 grad = float2(-dir.y, dir.x);

                        for (float i = -RANGE; i <= RANGE; i += STEP)
                        {
                            float2 b = normalize(dir);
                            float2 pos2 = screenPos + float2(b.x, b.y) * i;

                            // video texture wrap can't be set to anything other than clamp  (-_-)
                            if (pos2.y < 0.0 || pos2.x < 0.0 || pos2.x > _ScreenParams.x || pos2.y > _ScreenParams.y)
                                continue;

                            float2 g = getGrad(pos2, 1.0);

                            if (sqrt(dot(g,g)) < _GradThresh)
                                continue;

                            weight -= pow(abs(dot(normalize(grad), normalize(g))), _Senstivity) / floor((2.0 * RANGE + 1.0) / STEP) / _Samples;
                        }
                    }

                    float4 col = getCol(screenPos);
                    float4 background = lerp(col, float4(1.0, 1.0, 1.0, 1.0), _ColorThreshold);
                    // I couldn't get this to look good, but I guess it's almost obligatory at this point...
                    /*float distToLine = absCircular(fragCoord.y / (iResolution.y/8.));
                    background = mix(vec4(0.6,0.6,1,1), background, smoothstep(0., 0.03, distToLine));*/

                    half4 a = tex2D(_BackgroundTex, screenPos / _ScreenParams.xy);

                    return background - (1 - weight);
                    //return lerp(float4(0.0, 0.0, 0.0, 0.0), background, weight) - a.x * _BackgroundInt;
                }

                ENDCG
            }
        }
}