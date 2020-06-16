Shader "Custom/Alpha/NormalAlpha" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM

        //再最后添加alpha参数，就可以实现模型的透明设置
        #pragma surface surf Lambert alpha 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };


        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}

//Shader "Custom/SimpleAlpha" {    
//    Properties {    
//        _MainTex ("Base (RGB)", 2D) = "white" {}    
//        _TransVal ("Transparency Value", Range(0,1)) = 0.5    
//    }    
//    SubShader {    
//        Tags { "RenderType"="Opaque" "Queue"="Transparent"}    
//        LOD 200    
//            
//        CGPROGRAM    
//        #pragma surface surf Lambert alpha    
//    
//        sampler2D _MainTex;    
//        float _TransVal;    
//    
//        struct Input {    
//            float2 uv_MainTex;    
//        };    
//    
//        void surf (Input IN, inout SurfaceOutput o) {    
//            half4 c = tex2D (_MainTex, IN.uv_MainTex);    
//            o.Albedo = c.rgb;    
//            o.Alpha = c.b * _TransVal;    
//        }    
//        ENDCG    
//    }     
//    FallBack "Diffuse"    
//}  