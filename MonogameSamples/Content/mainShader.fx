//float4x4 World;
//float4x4 View;
//float4x4 Projection;

sampler TextureSampler : register(s0);

struct Light
{
	float3 position;
	float3 color;
	float invRadius;
};

struct PixelShaderOutput
{
	float4 diffuse : SV_Target0;
	float4 depth : SV_Target1;
};

texture test;

SamplerState testSampler {
	Texture = (test);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};


PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;
	output.diffuse = tex2D(TextureSampler, input.UV);
	output.depth = tex2D(testSampler, input.UV);
	return output;
}

technique Technique1
{
	pass Pass1
	{
		
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}


