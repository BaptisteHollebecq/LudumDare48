// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vfx/Props/Muret_CellShading"
{
	Properties
	{
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		_ColorCellShading("Color Cell Shading", Color) = (0.6037736,0.6037736,0.6037736,0)
		_ScaleNormal("Scale Normal", Range( 0 , 3)) = 0.8277993
		_LightIntensity("Light Intensity", Float) = 1.73
		[NoScaleOffset][Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[Toggle(_DOTORVIEWDIR_ON)] _DotorViewDir("Dot or View Dir", Float) = 0
		[Toggle(_LIGHTDIFFUSEORNOT_ON)] _LightDiffuseorNot("LightDiffuse or Not", Float) = 0
		_AmountPower("Amount Power", Range( 0.1 , 1)) = 1
		_LimitSaturate("Limit Saturate", Float) = 0
		_ViewShadows("View Shadows", Vector) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _LIGHTDIFFUSEORNOT_ON
		#pragma shader_feature_local _DOTORVIEWDIR_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _ColorCellShading;
		uniform sampler2D _Albedo;
		uniform sampler2D _NormalMap;
		uniform float _ScaleNormal;
		uniform float3 _ViewShadows;
		uniform float _LimitSaturate;
		uniform float _AmountPower;
		uniform float _LightIntensity;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 tex2DNode3 = UnpackScaleNormal( tex2D( _NormalMap, i.uv_texcoord ), _ScaleNormal );
			float3 normalizeResult21 = normalize( (WorldNormalVector( i , tex2DNode3 )) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult20 = normalize( ase_worldlightDir );
			float dotResult17 = dot( normalizeResult21 , normalizeResult20 );
			float temp_output_53_0 = saturate( dotResult17 );
			float dotResult76 = dot( reflect( normalizeResult21 , normalizeResult20 ) , _ViewShadows );
			#ifdef _DOTORVIEWDIR_ON
				float staticSwitch78 = ( temp_output_53_0 * max( ( 1.0 - dotResult76 ) , _LimitSaturate ) );
			#else
				float staticSwitch78 = temp_output_53_0;
			#endif
			float Dot58 = pow( staticSwitch78 , _AmountPower );
			UnityGI gi31 = gi;
			float3 diffNorm31 = WorldNormalVector( i , tex2DNode3 );
			gi31 = UnityGI_Base( data, 1, diffNorm31 );
			float3 indirectDiffuse31 = gi31.indirect.diffuse + diffNorm31 * 0.0001;
			float3 shadows60 = ( ( Dot58 * ( ( ase_lightAtten * Dot58 ) + indirectDiffuse31 ) ) + indirectDiffuse31 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 Light59 = ( float4( shadows60 , 0.0 ) * ( ase_lightColor * _LightIntensity ) );
			#ifdef _LIGHTDIFFUSEORNOT_ON
				float4 staticSwitch70 = UNITY_LIGHTMODEL_AMBIENT;
			#else
				float4 staticSwitch70 = Light59;
			#endif
			float4 LightParameter62 = ( staticSwitch70 * Light59 );
			c.rgb = saturate( ( ( _ColorCellShading * tex2D( _Albedo, i.uv_texcoord ) ) * LightParameter62 ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-1858;-1908;1858;1047;-2599.165;-3140.866;1.912452;True;False
Node;AmplifyShaderEditor.CommentaryNode;48;222.9153,2582.778;Inherit;False;1004.956;719.7371;Comment;4;66;65;5;3;Normal Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;5;374.8832,3172.071;Inherit;False;Property;_ScaleNormal;Scale Normal;2;0;Create;True;0;0;0;False;0;False;0.8277993;1.54;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;66;371.5172,2716.427;Inherit;True;Property;_NormalMap;Normal Map;4;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;406.5687,2974.534;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;50;1411.371,2603.744;Inherit;False;3093.337;1025.189;Comment;19;58;81;78;80;75;53;88;87;17;85;76;73;21;20;15;14;92;93;101;Dot;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;3;748.3447,2814.409;Inherit;True;Property;_Muret_Test_Final_Material_26_Normal;Muret_Test_Final_Material _26_Normal;1;1;[Normal];Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;14;1507.392,2760.497;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;15;1550.059,3028.33;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;21;1798.811,2762.373;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;20;1830.91,3024.84;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;92;2195.474,3405.817;Inherit;False;Property;_ViewShadows;View Shadows;9;0;Create;True;0;0;0;False;0;False;1,1,1;1,-1.18,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ReflectOpNode;73;2140.825,3164.4;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;76;2533.449,3258.289;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;2787.335,3368.859;Inherit;False;Property;_LimitSaturate;Limit Saturate;8;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;93;2771.191,3269.967;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;87;3005.261,3301.806;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;17;2181.195,2762.471;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;2473.747,2790.119;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;88;3216.324,3234.803;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;3270.195,3131.832;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;3442.018,3311.852;Inherit;False;Property;_AmountPower;Amount Power;7;0;Create;True;0;0;0;False;0;False;1;0.236;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;78;3517.489,3070.656;Inherit;False;Property;_DotorViewDir;Dot or View Dir;5;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;81;3867.317,3225.617;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;4207.622,3218.704;Inherit;True;Dot;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;42;730.1184,3735.705;Inherit;False;2029.668;752.3385;Comment;10;60;33;32;31;30;29;64;98;99;97;Shadows;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;886.1927,4086.041;Inherit;False;58;Dot;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;29;854.5677,3948.107;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;31;1074.856,4189.132;Inherit;True;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1122.562,4015.844;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;1367.449,4067.445;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;1711.071,4006.974;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;98;1742.041,4234.304;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;57;1796.618,4390.099;Inherit;False;935.623;331.6494;Comment;6;55;46;56;28;59;63;Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;2149.008,4025.642;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;56;1851.498,4593.515;Inherit;False;Property;_LightIntensity;Light Intensity;3;0;Create;True;0;0;0;False;0;False;1.73;3.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;28;1846.617,4440.099;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;2359.971,4061.162;Inherit;False;shadows;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;2072.8,4557.885;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;2041.647,4459.639;Inherit;False;60;shadows;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;2234.81,4465.729;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;71;1920.655,4814.918;Inherit;False;1184.039;479.1631;Comment;6;67;68;70;69;62;72;Light Diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;2485.78,4463.146;Inherit;False;Light;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;49;3078.586,3753.837;Inherit;False;1131.941;650.957;Comment;6;12;11;2;7;23;4;Base Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;67;2022.34,4904.583;Inherit;False;UNITY_LIGHTMODEL_AMBIENT;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;1970.653,5041.212;Inherit;True;59;Light;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;72;2439.684,5103.568;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;70;2342,4948.757;Inherit;False;Property;_LightDiffuseorNot;LightDiffuse or Not;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;3233.799,4204.394;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;2670.345,5040.568;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;3180.429,3938.989;Inherit;True;Property;_Albedo;Albedo;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WireNode;23;3541.539,4158.538;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;3601.935,4061.605;Inherit;True;Property;_Muret_Test_Final_Material_26_BaseColor;Muret_Test_Final_Material _26_BaseColor;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;2875.085,5035.682;Inherit;True;LightParameter;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;11;3642.234,3861.441;Inherit;False;Property;_ColorCellShading;Color Cell Shading;1;0;Create;True;0;0;0;False;0;False;0.6037736,0.6037736,0.6037736,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;61;4013.103,4465.173;Inherit;False;62;LightParameter;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;3990.521,4016.094;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;4335.774,4381.168;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;89;4643.285,4387.564;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;2811.855,3008.216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;99;2403.376,3913.346;Inherit;False; ;1;False;1;True;In0;FLOAT;0;In;;Inherit;False;My Custom Expression;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;2565.855,3043.216;Inherit;False;Property;_Float0;Float 0;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4944.958,4173.15;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Vfx/Props/Muret_CellShading;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;66;0
WireConnection;3;1;65;0
WireConnection;3;5;5;0
WireConnection;14;0;3;0
WireConnection;21;0;14;0
WireConnection;20;0;15;0
WireConnection;73;0;21;0
WireConnection;73;1;20;0
WireConnection;76;0;73;0
WireConnection;76;1;92;0
WireConnection;93;0;76;0
WireConnection;87;0;93;0
WireConnection;87;1;85;0
WireConnection;17;0;21;0
WireConnection;17;1;20;0
WireConnection;53;0;17;0
WireConnection;88;0;87;0
WireConnection;75;0;53;0
WireConnection;75;1;88;0
WireConnection;78;1;53;0
WireConnection;78;0;75;0
WireConnection;81;0;78;0
WireConnection;81;1;80;0
WireConnection;58;0;81;0
WireConnection;31;0;3;0
WireConnection;30;0;29;0
WireConnection;30;1;64;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;33;0;64;0
WireConnection;33;1;32;0
WireConnection;98;0;31;0
WireConnection;97;0;33;0
WireConnection;97;1;98;0
WireConnection;60;0;97;0
WireConnection;55;0;28;0
WireConnection;55;1;56;0
WireConnection;46;0;63;0
WireConnection;46;1;55;0
WireConnection;59;0;46;0
WireConnection;72;0;68;0
WireConnection;70;1;68;0
WireConnection;70;0;67;0
WireConnection;69;0;70;0
WireConnection;69;1;72;0
WireConnection;23;0;4;0
WireConnection;2;0;7;0
WireConnection;2;1;23;0
WireConnection;62;0;69;0
WireConnection;12;0;11;0
WireConnection;12;1;2;0
WireConnection;22;0;12;0
WireConnection;22;1;61;0
WireConnection;89;0;22;0
WireConnection;100;0;53;0
WireConnection;100;1;101;0
WireConnection;0;13;89;0
ASEEND*/
//CHKSM=D89B3EBDFBA17668FCEC56473170ECF84A2388BB