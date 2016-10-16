//float4x4 World;
//float4x4 View;
//float4x4 Projection;

sampler TextureSampler : register(s0);

 

struct PixelShaderOutput
{
	float4 depth : SV_Target0;
	float4 diffuse : SV_Target1;
	float4 normal : SV_Target2;
	float4 light : SV_Target3;
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
	output.depth = float4(1, 1, 1, 1);
	output.diffuse = float4(1, 1, 1, 1);
	output.normal = float4(0.5f, 0.5, 1, 1);
	output.light = float4(0.0, 0.0, 0.0, 1);
	return output;
}

technique Technique1
{
	pass Pass1
	{
		
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}


