using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Quick_Upload
{
	class Settings : ModSettings
	{
		public static bool TagWorkshopTitle = true;

		public static void DoSettingsWindowContents(Rect rect)
		{
			Listing_Standard options = new Listing_Standard();

			options.Begin(rect);

			options.Gap(20f);

			options.CheckboxLabeled("Add game version to item title", ref TagWorkshopTitle, "Will prefix the mod title on Steam with the current target version from About.xml ('[1.0] Quick Upload')");

			options.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();

			Scribe_Values.Look(ref TagWorkshopTitle, "QU_TagWorkshopTitle", true);
		}
	}
}
