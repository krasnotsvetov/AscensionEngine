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
	float4 depth : SV_Target0;
	float4 diffuse : SV_Target1;
	float4 normal : SV_Target2;
	float4 light : SV_Target3;
};

texture NormalMap;

SamplerState NormalSampler {
	Texture = (NormalMap);
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
	output.depth = input.Position.z / input.Position.w;
	output.diffuse = tex2D(TextureSampler, input.UV) * (1, 0, 0, 1);// (input.Position.X, input.Position.Y, input.Position.X * input.Position.Y, 1);
	output.normal = tex2D(NormalSampler, input.UV) ;
	return output;
}

technique Technique1
{
	pass Pass1
	{
		
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}


