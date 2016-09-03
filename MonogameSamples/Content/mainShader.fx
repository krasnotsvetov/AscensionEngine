//float4x4 World;
//float4x4 View;
//float4x4 Projection;

sampler TextureSampler : register(s0);


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
	float4 Position : SV_Position;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

	float4 tex = tex2D(TextureSampler, input.UV);
	if (input.UV.x < 0.5f) {
		return tex2D(testSampler, input.UV);
	}
	return tex;
}

technique Technique1
{
	pass Pass1
	{
		
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}


