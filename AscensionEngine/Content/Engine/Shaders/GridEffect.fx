float4x4 World;
float4x4 View;
float4x4 Projection;
float4 CameraPos;

struct PixelShaderOutput
{
	float4 depth : SV_Target0;
	float4 diffuse : SV_Target1;
	float4 normal : SV_Target2;
	float4 light : SV_Target3;
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float3 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Color : COLOR0;

};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Color = input.Color;
	return output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;
	output.depth = input.Position.z / input.Position.w;
	output.diffuse = float4(input.Color, 0.1);
	output.light = float4(1, 1, 1, 1);
	output.normal = float4(0.5, 0.5, 1, 1);
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


