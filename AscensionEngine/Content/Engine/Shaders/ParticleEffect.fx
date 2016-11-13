float4x4 World;
float4x4 View;
float4x4 Projection;

float CurrentTime;
float2 ViewportScale;

float Duration;
float3 Gravity;
float DurationFactor;
float FadeFactor;

float EndVelocity;
float4 MinColor;
float4 MaxColor;

float2 RotationSpeed;
float2 StartSize;
float2 EndSize;

sampler TextureSampler : register(s0);


texture Albedo;
sampler2D AlbedoSampler = sampler_state
{
	Texture = <Albedo>;
	MipFilter = POINT;
	MinFilter = POINT;
	MagFilter = POINT;
};

texture NormalMap;

SamplerState NormalSampler {
	Texture = (NormalMap);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct PixelShaderOutput
{
	float4 depth : SV_Target0;
	float4 diffuse : SV_Target1;
	float4 normal : SV_Target2;
	float4 light : SV_Target3;
};


struct VertexShaderInput
{
	float4 Position : SV_POSITION0;
	float3 Velocity : NORMAL0;
	float2 Corner   : TEXCOORD0;
	float4 Random : TEXCOORD1;
	float Time : TEXCOORD2;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinate : COLOR1;
};



float4 GetParticlePosition(float4 position, float3 velocity, float age, float normalizedAge)
{
	float startVelocity = length(velocity);
	float endVelocity = startVelocity * EndVelocity;

	//x = v_0 * t + a * t^2 / 2 
	float velocityIntegral = startVelocity * normalizedAge + (endVelocity - startVelocity) * normalizedAge * normalizedAge / 2;

	position += float4(normalize(velocity) * velocityIntegral * Duration, 0);

	position += float4(Gravity * age * normalizedAge, 0);

	return position;
}

float ComputeParticleSize(float randomValue, float normalizedAge)
{
	float startSize = lerp(StartSize.x, StartSize.y, randomValue);
	float endSize = lerp(EndSize.x, EndSize.y, randomValue);

	float size = lerp(startSize, endSize, normalizedAge);

	return size;
}

float4 ComputeParticleRotation(float randomValue, float age)
{
	float rotateSpeed = lerp(RotationSpeed.x, RotationSpeed.y, randomValue);

	float rotation = rotateSpeed * age;

	float c = cos(rotation);
	float s = sin(rotation);

	return float2x2(c, -s, s, c);
}


float4 ComputeParticleColor(float4 projectedPosition,
	float randomValue, float normalizedAge)
{
	float4 color = lerp(MinColor, MaxColor, randomValue);


	float fade = normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * FadeFactor;
	if (fade > 1)
	{
		fade = 1;
	}
	color.a *= fade;

	return color;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float age = CurrentTime - input.Time;
	age *= 1 + input.Random.x * DurationFactor;
	float normalizedAge = saturate(age / Duration);
	float size = ComputeParticleSize(input.Random.y, normalizedAge);
	float2x2 rotation = ComputeParticleRotation(input.Random.w, age);
	input.Position.xy += mul(input.Corner, rotation) * size;
    float4 worldPosition = mul(GetParticlePosition(input.Position, input.Velocity, age, normalizedAge), World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.Color =  ComputeParticleColor(output.Position, input.Random.z, normalizedAge);
	output.TextureCoordinate = (input.Corner + float2(1, 1)) / 2;

	return output;
}

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
	PixelShaderOutput output;
	//output.depth = input.Position.z / input.Position.w;
	output.diffuse = tex2D(AlbedoSampler, input.TextureCoordinate) * input.Color;
	//output.normal = tex2D(NormalSampler, input.UV);
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


