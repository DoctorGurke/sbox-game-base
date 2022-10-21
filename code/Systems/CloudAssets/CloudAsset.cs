using System.Threading.Tasks;

namespace GameBase.Systems.CloudAssets;

public static class CloudAsset
{
	/// <summary>
	/// Load a model from asset.party.
	/// </summary>
	/// <param name="packageOrigin">The asset.party package.</param>
	/// <param name="source">The source entity for this asset request.</param>
	/// <param name="fallback">The fallback model path, defaults to error model.</param>
	/// <returns>The loaded Model.</returns>
	public static Model LoadModel( string packageOrigin, Entity source, string fallback = $"models/dev/error.vmdl" )
	{
		var path = LoadCloudAsset( packageOrigin, source, Package.Type.Model, fallback );
		var model = Model.Load( path.Result );
		return model;
	}

	/// <summary>
	/// Load a material from asset.party.
	/// </summary>
	/// <param name="packageOrigin">The asset.party package.</param>
	/// <param name="source">The source entity for this asset request.</param>
	/// <param name="fallback">The fallback material path, defaults to error material.</param>
	/// <returns>The loaded Material.</returns>
	public static Material LoadMaterial( string packageOrigin, Entity source, string fallback = $"materials/error.vmat" )
	{
		var path = LoadCloudAsset( packageOrigin, source, Package.Type.Material, fallback );
		var material = Material.Load( path.Result );
		return material;
	}

	/// <summary>
	/// Load a sound from asset.party.
	/// </summary>
	/// <param name="packageOrigin">The asset.party package.</param>
	/// <param name="source">The source entity for this asset reuest.</param>
	/// <param name="fallback">The fallback sound path, defaults to </param>
	/// <returns></returns>
	public static string LoadSound( string packageOrigin, Entity source, string fallback = $"sounds/player_use_fail.sound" )
	{
		var path = LoadCloudAsset( packageOrigin, source, Package.Type.Sound, fallback );
		return path.Result ?? fallback;
	}

	private static async Task<string?> LoadCloudAsset( string packageOrigin, Entity source, Package.Type type = Package.Type.Any, string? fallback = null )
	{
		var package = await Package.Fetch( packageOrigin, false );

		// invalid package, invalid type, no upload
		if ( package is null || package.PackageType != type || package.Revision is null )
		{
			return null;
		}

		// source entity is gone, bail
		if ( !source.IsValid ) return null;

		var asset = package.GetMeta( "PrimaryAsset", fallback );

		// download and/or mount if needed
		await package.MountAsync();

		return asset;
	}
}
