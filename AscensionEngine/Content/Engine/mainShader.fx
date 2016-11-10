float4x4 World;
float4x4 View;
float4x4 Projection;

sampler TextureSampler : register(s0);


texture Albedo;
sampler2D AlbedoSampler = sampler_state
{
	Texture = <Albedo>;
	MipFilter = POINT;
	MinFilter = POINT;
	MagFilter = POINT;
};


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

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL0;
	float2 UV : TEXCOORD0;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, World);
	output.UV = input.UV;
	return output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;
	float4 baseColor = tex2D(AlbedoSampler, input.UV);
	clip((baseColor.a == 0) ? -1 : 1);

	output.diffuse = tex2D(AlbedoSampler, input.UV);
	output.normal = tex2D(NormalSampler, input.UV);
	output.depth = input.Position.z / input.Position.w;

	return output;
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}


