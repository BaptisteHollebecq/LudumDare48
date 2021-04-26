// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.71
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		[HDR]_SecondaryColor("Secondary Color", Color) = (0,0,0,0)
		_MainColorExtend("MainColorExtend", Float) = 0
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv2_tex4coord2;
			float4 uv_tex4coord;
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform float4 _SecondaryColor;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _MainColorExtend;
		uniform float _Cutoff = 0.71;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_8_0 = ( i.uv_tex4coord.z + i.uv_tex4coord.w );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode1 = tex2D( _TextureSample0, uv_TextureSample0 );
			float smoothstepResult9 = smoothstep( i.uv_tex4coord.z , temp_output_8_0 , tex2DNode1.r);
			float smoothstepResult14 = smoothstep( i.uv_tex4coord.z , ( temp_output_8_0 + _MainColorExtend ) , tex2DNode1.r);
			float4 lerpResult10 = lerp( ( _MainColor * i.uv2_tex4coord2 ) , ( _SecondaryColor * i.uv2_tex4coord2 ) , ( smoothstepResult9 + smoothstepResult14 ));
			o.Emission = lerpResult10.rgb;
			o.Alpha = 1;
			clip( smoothstepResult9 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-1582;-1847;1160;691;1852.072;717.5306;2.125333;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1594.847,547.1394;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-909.226,592.8748;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1000.853,62.21299;Inherit;False;Property;_MainColorExtend;MainColorExtend;6;0;Create;True;0;0;0;False;0;False;0;19.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1136.508,-205.9007;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;9acbfe9cb183baa48821cfb989e2d493;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-630.8541,169.7934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-739.1498,-423.834;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-486.1811,-501.1259;Inherit;False;Property;_MainColor;Main Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;5.340313,4.289306,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-455.8237,-311.2585;Inherit;False;Property;_SecondaryColor;Secondary Color;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,0.1361872,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;9;-622.9925,406.3011;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;14;-660.6982,-79.95529;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-196.212,-212.7526;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-257.6284,217.4617;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-175.4934,-370.2214;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1394.036,409.7514;Inherit;False;Property;_Position1;Position;4;0;Create;True;0;0;0;False;0;False;0.74;0.286;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-5.489464,-101.4293;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1350.173,680.7312;Inherit;False;Property;_Contrast1;Contrast;3;0;Create;True;0;0;0;False;0;False;0.66;0.565;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;228.3484,236.501;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.71;True;True;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;13;3
WireConnection;8;1;13;4
WireConnection;15;0;8;0
WireConnection;15;1;18;0
WireConnection;9;0;1;1
WireConnection;9;1;13;3
WireConnection;9;2;8;0
WireConnection;14;0;1;1
WireConnection;14;1;13;3
WireConnection;14;2;15;0
WireConnection;21;0;12;0
WireConnection;21;1;19;0
WireConnection;16;0;9;0
WireConnection;16;1;14;0
WireConnection;20;0;3;0
WireConnection;20;1;19;0
WireConnection;10;0;20;0
WireConnection;10;1;21;0
WireConnection;10;2;16;0
WireConnection;0;2;10;0
WireConnection;0;10;9;0
ASEEND*/
//CHKSM=DB294154ADB75244F4ABA623BA3F5111F9FDC490