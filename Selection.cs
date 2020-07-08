using System;
using Sifteo;
using System.Collections;
using System.Collections.Generic;

namespace KYMA_SIFTEO
{
	public class Selection
	{
		private KYMA_SIFTEO Selection_kyma1;
		private List<CubeWrapper> mWrappers1 = new List<CubeWrapper>();
		private CubeSet cubeset1;


		private int NOTHING = 0;
		private int SOURCEWITHOUTLS = 1;
		private int SOURCE = 2;
		private int SOURCELOWFILTER = 3;
		private int SOURCEHIGHFILTER = 4;
		private int SOURCEFILTERLFO = 5;
		private int SOURCEFILTERLFOSPECIAL = 6;

		public Selection (KYMA_SIFTEO kym, List<CubeWrapper> wrapper, CubeSet cubeset)
		{
			Selection_kyma1 = kym;
			mWrappers1 = wrapper;
			cubeset1 = cubeset;

		}
		//Hesitation, une seule méthode avec plusieurs arguments ou plusieurs méthodes
		public Selection (int source, int hp, int filter, int lfo)
		{

		} 
	}
}

