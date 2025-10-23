using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace realisticThermalScopes
{
	public record ModMetadata : AbstractModMetadata
	{
		public override string Name { get; init; } = "realisticThermalScopes";
		public override string Author { get; init; } = "FiveF";
		public override List<string>? Contributors { get; init; }
		public override SemanticVersioning.Version Version { get; init; } = new("2.0.0");
		public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");


		public override List<string>? Incompatibilities { get; init; }
		public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
		public override string? Url { get; init; }
		public override bool? IsBundleMod { get; init; } = true;
		public override string? License { get; init; } = "MIT";
		public override string ModGuid { get; init; } = "com.fivef.realisticthermalscopes";
	}

	[Injectable(TypePriority = OnLoadOrder.PostSptModLoader)]
	public class editBundle(ISptLogger<editBundle> logger) : IOnLoad
	{
		public Task OnLoad()
		{

			return Task.CompletedTask;
		}
	}



	[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
	public class editDatabase(
	ISptLogger<editDatabase> logger, DatabaseService databaseService) : IOnLoad
	{
		public Task OnLoad()
		{
			changeSensForTherScopes();

			return Task.CompletedTask;
		}

		private void changeSensForTherScopes()
		{
			var itemsTable = databaseService.GetTables().Templates.Items;

			//FLIR RS-32 2.25-9x 35mm 60Hz thermal riflescope
			var flirProps = itemsTable["5d1b5e94d7ad1a2b865a96b0"].Properties;
			double fovZoomInGame = 0.55; //~0.05 spt3.11
			flirProps.Zooms = new List<List<double>> { new List<double> { 2.25, 9 } };
			flirProps.AimSensitivity = new List<List<double>>
			{
				new()
				{
					fovZoomInGame / ((List<List<double>>)flirProps.Zooms)[0][0],
					fovZoomInGame / ((List<List<double>>)flirProps.Zooms)[0][1]
				}
			};

			//SIG Sauer ECHO1 1-2x30mm 30Hz thermal reflex scope
			var echo1Props = itemsTable["6478641c19d732620e045e17"].Properties;
			fovZoomInGame = 0.564; //~0.05 spt3.11
			echo1Props.Zooms = new List<List<double>> { new List<double> { 1, 2, 1, 2 } };
			var echo1PropsTrueZoom = new List<double> { 1.75, 3.5, 1.75, 3.5 };
			var newEcho1AimSensitivity = new List<double>
			{
				fovZoomInGame / echo1PropsTrueZoom[0],
				fovZoomInGame / echo1PropsTrueZoom[1],
				fovZoomInGame / echo1PropsTrueZoom[2],
				fovZoomInGame / echo1PropsTrueZoom[3]
			};
			echo1Props.AimSensitivity = new List<List<double>> { newEcho1AimSensitivity };

			//Torrey Pines Logic T12W 30Hz thermal reflex sight
			var t12Props = itemsTable["609bab8b455afd752b2e6138"].Properties;
			fovZoomInGame = 0.72; //~0.05 spt3.11
			t12Props.Zooms = new List<List<double>> { new List<double> { 1 } };
			t12Props.AimSensitivity = new List<List<double>>
			{
				new()
				{
					fovZoomInGame / ((List<List<double>>)t12Props.Zooms)[0][0]
				}
			};

			//Armasight Zeus-Pro 640 2-8x50 30Hz thermal scope
			var zeusProps = itemsTable["63fc44e2429a8a166c7f61e6"].Properties;
			fovZoomInGame = 0.55; //~0.05 spt3.11
			zeusProps.Zooms = new List<List<double>> { new List<double> { 2, 16, 2, 16 } };
			zeusProps.AimSensitivity = new List<List<double>>
			{
				new()
				{
					fovZoomInGame / ((List<List<double>>)zeusProps.Zooms)[0][0],
					fovZoomInGame / ((List<List<double>>)zeusProps.Zooms)[0][1],
					fovZoomInGame / ((List<List<double>>)zeusProps.Zooms)[0][2],
					fovZoomInGame / ((List<List<double>>)zeusProps.Zooms)[0][3]
				}
			};

			//Cyclone Shakhin 3.7x thermal scope
			var shakhinProps = itemsTable["67641b461c2eb66ade05dba6"].Properties;
			fovZoomInGame = 0.5423; //~0.05 spt3.11
			shakhinProps.Zooms = new List<List<double>> { new List<double> { 3.7 } };
			shakhinProps.AimSensitivity = new List<List<double>>
			{
				new()
				{
					fovZoomInGame / ((List<List<double>>)shakhinProps.Zooms)[0][0]
				}
			};

			//Trijicon REAP-IR thermal scope
			var reapProps = itemsTable["5a1eaa87fcdbcb001865f75e"].Properties;
			fovZoomInGame = 0.55; //~0.05 spt3.11
			reapProps.Zooms = new List<List<double>> { new List<double> { 9.6, 1.2 } };
			reapProps.AimSensitivity = new List<List<double>>
			{
				new()
				{
					fovZoomInGame / ((List<List<double>>)reapProps.Zooms)[0][0],
					fovZoomInGame / ((List<List<double>>)reapProps.Zooms)[0][1]
				}
			};

			/*
			const ultimaThrmProps = itemsData[`606f2696f2cb2e02a42aceb1`]._props
			fovZoomInGame = ; //~0.05 spt3.11
			ultimaThrmProps.Zooms[0] = ;
			ultimaThrmProps.AimSensitivity[0] = [fovZoomInGame / ultimaThrmProps.Zooms[0][0], fovZoomInGame / ultimaThrmProps.Zooms[0][1]];
			*/


			/*
			
			//this
			zeusProps.AimSensitivity = new List<List<double>>
			{
				new() { ... },
				new() { ... }
			};
			//for this
			"AimSensitivity": [
			  [ ... ],
			  [ ... ]
			]
			*/

		}
	}

}
