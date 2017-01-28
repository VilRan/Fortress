#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
texture Texture;
float4 AmbientColor;
float LightLevel;

sampler2D textureSampler = sampler_state {
	Texture = (Texture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate : TEXCOORD0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate : TEXCOORD0;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.TextureCoordinate = input.TextureCoordinate;

	float dy = abs(LightLevel - input.Position.y);
	float v = 1 / (1 + dy);
	output.Color = input.Color * AmbientColor * float4(v, v, v, 1);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);

	return textureColor * input.Color;
}

technique DefaultShader
{
	pass P0
	{
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};