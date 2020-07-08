
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Sifteo;
using Sifteo.Util;


namespace KYMA_SIFTEO {

		public class KYMA_SIFTEO : BaseApp {

	    public String[] mImageNames;
	    public List<CubeWrapper> mWrappers = new List<CubeWrapper>();
	    public Random mRandom = new Random();
		public static int  spritewidth = 128;

		public static Cube SOURCE;
		public static Cube LS;
		public static Cube FILTER;
		public static Cube LFO;
		public static Cube SPECIAL;
		public static Cube [] Cubes = {SOURCE, LS, FILTER, LFO, SPECIAL};

		public static int sourcei;
		public static int lsi;
		public static int filteri;
		public static int lfoi;
		public static int speciali;
		public static int [] cubesint = {sourcei, lsi, filteri, lfoi, speciali};

/*	public static bool sourceisconnected = false;
	public static bool lsisconnected = false;
	public static bool filterisconnected = false;
	public static bool lfohisconnected = false;
	public static bool specialisconnected = false;
	public static bool[] cubesisconnected = {sourceisconnected,lsisconnected,filterisconnected,lfohisconnected,specialisconnected};
		
	public static bool sourcehaschanged = false;
	public static bool lshaschanged = false;
	public static bool filterhaschanged = false;
	public static bool lfohaschanged = false;
	public static bool specialhaschanged = false;
	public static bool[] cubeshaschanged = {sourcehaschanged,lshaschanged,filterhaschanged,lfohaschanged,specialhaschanged};
*/
		public static int  FULL_SOURCE1 = (spritewidth*0);
		public static int  FULL_SOURCE2 = (spritewidth*1);
		public static int  FULL_SOURCE3 = (spritewidth*2);
		public static int  FULL_SOURCE4 = (spritewidth*3);
		public static int  FULL_LSON = (spritewidth*4);
		public static int  FULL_LSOFF = (spritewidth*5);
		public static int  FULL_LOWPASSFILTER = (spritewidth*6);
		public static int  FULL_HIGHPASSFILTER = (spritewidth*7);
		public static int  FULL_FILTER = (spritewidth*8);
		public static int  FULL_LFO1 = (spritewidth*9);
		public static int  FULL_LFO2 = (spritewidth*10);
		public static int  FULL_SPECIAL = (spritewidth*11);
		public static int  FULL_SPECIAL2 = (spritewidth*12);
		public static int  FULL_SOURCEALL = (spritewidth*13);
		public static int  FULL_MODULATION = (spritewidth*14);

		int sample = 0;
		public  OSC osc1 = new OSC("172.30.8.16",8000);
		public static bool mNeedCheck;
		int[] whoison = new int [] {0,0,0,0};
			/*
			public readonly int SOURCEON = 1;
			public readonly int SOURCEOFF = 0;
			public readonly int LSOFF = 0;
			public readonly int LSON = 1;
			public readonly int LFOOFF = 0;
			public readonly int LFOON = 1;
			public readonly int FILTEROFF = 0;
			public readonly int FILTERLOWPASS = 1;
			public readonly int FILTERHIGHPASS = 2;
			public readonly int EFFET = 0;
			public readonly int DELAY = 1;


			public int SOURCE = 0;
			public int LS = 0;
			public int FILTER = 0;
			public int LFO = 0;
			public int SPECIAL = 0;
			*/
			// Here we initialize our app.

    public override void Setup() 
	{
			// Load up the list of images.
     // mImageNames = LoadImageIndex();
			int number = 1;
			int state = 0;
			int otherstate =0;
			int j = 0;

			bool isconnedted = false;
			bool haschanged = false;
			WithdrawSound();

      // Loop through all the cubes and set them up.
      foreach (Cube cube in CubeSet) 
	  {
				Cubes[j] = cube;
        // Create a wrapper object for each cube. The wrapper object allows us
        // to bundle a cube with extra information and behavior.
        		CubeWrapper wrapper = new CubeWrapper(this, cube,number,state, otherstate,isconnedted,haschanged);
				mWrappers.Add(wrapper);
				cube.FillRect (new Color (0, 0, 255), 0, 0,Cube.SCREEN_WIDTH, Cube.SCREEN_HEIGHT);

				switch (wrapper.mNumber)
				{
				case 1:
					cube.Image("FULL",0,0,0,FULL_SOURCE1,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					break;
				case 2:
					cube.Image("FULL",0,0,0,FULL_LSOFF,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					break;
				case 3:
					cube.Image("FULL",0,0,0,FULL_FILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					break;
				case 4:
					cube.Image("FULL",0,0,0,FULL_LFO1,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					break;
				case 5:
					cube.Image("FULL",0,0,0,FULL_SPECIAL,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					break;
				}
				cube.Paint();
				number++;
				j++;
				//wrapper.DrawSlide();
       }

      // ## Event Handlers ##
      // Objects in the Sifteo API (particularly BaseApp, CubeSet, and Cube)
      // fire events to notify an app of various happenings, including actions
      // that the player performs on the cubes.
      //
      // To listen for an event, just add the handler method to the event. The
      // handler method must have the correct signature to be added. Refer to
      // the API documentation or look at the examples below to get a sense of
      // the correct signatures for various events.
      //
      // **NeighborAddEvent** and **NeighborRemoveEvent** are triggered when
      // the player puts two cubes together or separates two neighbored cubes.
      // These events are fired by CubeSet instead of Cube because they involve
      // interaction between two Cube objects. (There are Cube-level neighbor
      // events as well, which comes in handy in certain situations, but most
      // of the time you will find the CubeSet-level events to be more useful.)	
      CubeSet.NeighborAddEvent += OnNeighborAdd;
      CubeSet.NeighborRemoveEvent += OnNeighborRemove;
    }

		/*
    // ## Neighbor Add ##
    // This method is a handler for the NeighborAdd event. It is triggered when
    // two cubes are placed side by side.
    //
    // Cube1 and cube2 are the two cubes that are involved in this neighboring.
    // The two cube arguments can be in any order; if your logic depends on
    // cubes being in specific positions or roles, you need to add logic to
    // this handler to sort the two cubes out.
    //
    // Side1 and side2 are the sides that the cubes neighbored on.
    private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;
			Log.Debug ("Neighbor add: {0}.{1} <-> {2}.{3}", wrapper1.mNumber, side1, wrapper2.mNumber, side2);

			if (wrapper2.mNumber < wrapper1.mNumber) {
				CubeWrapper wrapper3 = cube1.userData;
				wrapper2 = wrapper1;
				wrapper1 = wrapper3;
			}



			//s'arranger pour savoir quel cube est le premier, celui qui a le plus petit nombre, devient wrapper1
			if (cube1 == SOURCE) {
				switch (cube2){
				case FILTER:
					break;
				}
				
				
			}


			if (wrapper1.mNumber == 1) {
				if (wrapper2.mNumber == 2) {
				
				}
			}

			if (wrapper1.mNumber== 2) {
				if (wrapper2.mNumber == 3) {
					cube2.Image("all",0,0,0,ALL_LFO,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
				}
			}

			cube1.Paint();
			cube2.Paint();
			/*
      CubeWrapper wrapper = (CubeWrapper)cube1.userData;
      if (wrapper != null) {
        // Here we set our wrapper's rotation value so that the image gets
        // drawn with its top side pointing towards the neighbor cube.
        //
        // Cube.Side is an enumeration (TOP, LEFT, BOTTOM, RIGHT, NONE). The
        // values of the enumeration can be cast to integers by counting
        // counterclockwise:
        //
        // * TOP = 0
        // * LEFT = 1
        // * BOTTOM = 2
        // * RIGHT = 3
        // * NONE = 4
        wrapper.mRotation = (int)side1;
        wrapper.mNeedDraw = true;
      }

      wrapper = (CubeWrapper)cube2.userData;
      if (wrapper != null) {
        wrapper.mRotation = (int)side2;
        wrapper.mNeedDraw = true;

    }

    // ## Neighbor Remove ##
    // This method is a handler for the NeighborRemove event. It is triggered
    // when two cubes that were neighbored are separated.
    //
    // The side arguments for this event are the sides that the cubes
    // _were_ neighbored on before they were separated. If you check the
    // current state of their neighbors on those sides, they should of course
    // be NONE.
    private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
    		CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;

			if (wrapper2.mNumber < wrapper1.mNumber) {
				CubeWrapper wrapper3 = cube1.userData;
				wrapper2 = wrapper1;
				wrapper1 = wrapper3;
			}
			Log.Debug ("Neighbor remove: {0}.{1} <-> {2}.{3}", wrapper1.mNumber, side1, wrapper2.mNumber, side2);

			if (cube1 == SOURCE) {
				switch (cube2){
				case FILTER:
					break;
				}
			
			
			}


			if (wrapper1.mNumber == 1) {
				if (wrapper2.mNumber == 2) {
					cube2.Image("all",0,0,0,ALL_FILTERBASIC,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					cube1.Image("all",0,0,0,ALL_SOURCE,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					//message OSC, son de base
				}
			}
			
			if (wrapper1.mNumber == 2) {
				if (wrapper2.mNumber == 3) {
					cube2.Image("all",0,0,0,ALL_LFOBASIC,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
					// message OSC, son filtré simplement
					//savoir si on sait faire un "ctrl+z" sur KYMA, alors on reprend juste le son précédent
					//si pas -> switch
				}
			}

			cube1.Paint();
			cube2.Paint();

    /*  CubeWrapper wrapper = (CubeWrapper)cube1.userData;
      if (wrapper != null) {
        wrapper.mScale = 1;
        wrapper.mRotation = 0;
        wrapper.mNeedDraw = true;
      }

      wrapper = (CubeWrapper)cube2.userData;
      if (wrapper != null) {
        wrapper.mScale = 1;
        wrapper.mRotation = 0;
        wrapper.mNeedDraw = true;
      } 
    }
		*/
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;

			if (wrapper2.mNumber < wrapper1.mNumber) 
			{
				CubeWrapper wrapper3 = (CubeWrapper)cube2.userData;
				wrapper2 = wrapper1;
				wrapper1 = wrapper3;

				Cube cuby;
				cuby = cube2;
				cube2 = cube1;
				cube1 = cuby;
			}

			Cube[] connected = CubeHelper.FindConnected (cube1);

			foreach (Cube cuby in connected) 
			{
				Log.Debug ("connected {0}", connected);
			}

			if (connected.Length > 1) 
			{
				foreach (Cube cubelook in connected) 
				{
					CubeWrapper wrapper4 = (CubeWrapper)cubelook.userData;

					if (wrapper4.mNumber == 1) 
					{
						wrapper2.mIsconnected = true;
						wrapper2.mHaschanged = true;
						mNeedCheck = true;
						Log.Debug("on a bien changé le connected");
					}
				}
			}
			mNeedCheck = true;
		}
		
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			bool sourceishere = false;
			bool sourceishere2 = false;
			//the "remove part" is a little bit more tricky because we have to check which part stays with the source
		/*
			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;
			
			if (wrapper2.mNumber < wrapper1.mNumber) 
			{
				CubeWrapper wrapper3 = (CubeWrapper)cube2.userData;
				wrapper2 = wrapper1;
				wrapper1 = wrapper3;

				Cube cuby = Cube;
				cuby = cube2;
				cube2 = cube1;
				cube1 = cuby;
			}
		*/
			Cube[] connected = CubeHelper.FindConnected (cube1);
			Cube[] connected2 = CubeHelper.FindConnected (cube2);

			foreach (Cube cubelook4 in connected) 
			{
				CubeWrapper wrapper4 = (CubeWrapper)cubelook4.userData;
				
				if (wrapper4.mNumber == 1) 
				{ //we check if the source is in this part, and if it is the case, everycube in the group is connected
					sourceishere = true;
				}
			}

			foreach (Cube cubelook2 in connected2) 
			{
				CubeWrapper wrapper4 = (CubeWrapper)cubelook2.userData;
				
				if (wrapper4.mNumber == 1) 
				{ //we check if the source is in this part, and if it is the case, everycube in the group is connected
					sourceishere2 = true;
				}
			}


			if (sourceishere) 
			{ 
				foreach (Cube cubelook3 in connected) 
				{
					CubeWrapper wrapper5 = (CubeWrapper)cubelook3.userData;
					wrapper5.mIsconnected = true;//if it is not the case, they are not connected
					wrapper5.mHaschanged = true;
				}
				foreach (Cube cubelook3 in connected2) 
				{
					CubeWrapper wrapper5 = (CubeWrapper)cubelook3.userData;
					wrapper5.mIsconnected = false;//if it is not the case, they are not connected
					wrapper5.mHaschanged = true;
				}
			}

			if (sourceishere2) 
			{ 
				foreach (Cube cubelook3 in connected2) 
				{
					CubeWrapper wrapper5 = (CubeWrapper)cubelook3.userData;
					wrapper5.mIsconnected = true;//if it is not the case, they are not connected
					wrapper5.mHaschanged = true;
				}
				foreach (Cube cubelook3 in connected) 
				{
					CubeWrapper wrapper5 = (CubeWrapper)cubelook3.userData;
					wrapper5.mIsconnected = false;//if it is not the case, they are not connected
					wrapper5.mHaschanged = true;
				}
			}
			/*foreach (Cube cubelook2 in connected2) 
			{
				CubeWrapper wrapper4 = (CubeWrapper)cubelook2.userData;
	
				if (wrapper4.mNumber == 1) 
				{ //we check if the source is in this part, and if it is the case, everycube in the group is connected
					foreach (Cube cubelook3 in connected2) 
					{
						CubeWrapper wrapper5 = (CubeWrapper)cubelook3.userData;
						wrapper5.mIsconnected = true;
						sourceishere = true;
					}
				}
			}*/
			mNeedCheck=true;
		}

			
    // Defer all per-frame logic to each cube's wrapper.
    public override void Tick ()
		{
			if (mNeedCheck) 
			{
				mNeedCheck = false;
				CheckWhoChanged();

				foreach (Cube cuby in Cubes)
				{
					cuby.Paint();
				}
			}
   		 }

		public void CheckWhoChanged ()
		{   
			foreach (CubeWrapper wrapper in mWrappers) 
			{
				Cube cubecube;
				cubecube = wrapper.mCube;
				if(wrapper.mHaschanged)//we check if something has changed 
				{ 
				//Log.Debug ("est passé par haschanged");
					if (wrapper.mNumber == 1) 
					{ //the source, is always "connected"
					//Log.Debug ("state du cube {0}", wrapper.mState);
						switch (wrapper.mState) 
						{
							case 0:
								sample = 1;
							cubecube.Image("FULL",0,0,0,FULL_SOURCE1,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
								//Log.Debug ("on a changé de sample");
									//sample 1
									//osc1.sendMsg("/vcs",3145733,50f);
								break;
							case 1:
							cubecube.Image("FULL",0,0,0,FULL_SOURCE2,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
							//	Log.Debug ("on a changé sur le sample 1");
								sample = 2;
									//sample 2
								break;
							case 2:
							cubecube.Image("FULL",0,0,0,FULL_SOURCE3,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
								//Log.Debug ("on a changé de sample");
								sample = 3;
									//sample 3
								break;
							case 3:
							cubecube.Image("FULL",0,0,0,FULL_SOURCE4,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
								//Log.Debug ("on a changé de sample");
								sample = 4;
									//sample 4
								break;
						}
					}

					/*Log.Debug ("est passe par ici");
					Log.Debug ("sample {0}", sample);*/
					if(wrapper.mIsconnected) //here we check what we send to kyma
					{
					Log.Debug ("on est passé par les connectés");
					Log.Debug ("numero du cube {0}", wrapper.mNumber);
					Log.Debug ("state du cube {0}", wrapper.mState);
						switch (wrapper.mNumber) 
						{
							case 2:
								switch (wrapper.mState) 
								{
									case 1:
									WithdrawSound();
								cubecube.Image("FULL",0,0,0,FULL_LSOFF,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									break;

									case 0:
								/*	Log.Debug ("  sound");
									Log.Debug ("entrée dans le hautparleur");*/
										switch (sample) 
										{
											//LS sound
											case 1:
									cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												Log.Debug ("on envoie du son");
												osc1.sendMsg ("/vcs", 3145731, 0.9999f);
											System.Threading.Thread.Sleep(50);
												break;
											case 2:
									cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												osc1.sendMsg ("/vcs", 3145732, 0.9999f);
											System.Threading.Thread.Sleep(50);
												break;
											case 3:
									cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												osc1.sendMsg ("/vcs", 3145733, 0.9999f);
											System.Threading.Thread.Sleep(50);
												break;
											case 4:
									cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												osc1.sendMsg ("/vcs", 3145734, 0.9999f);
											System.Threading.Thread.Sleep(50);
												break;
										}
										//osc1.sendMsg("/vcs",3145731,
									break;
							     }
								break;

					    case 3:
							switch (wrapper.mState) 
							{
								case 0:
								cubecube.Image("FULL",0,0,0,FULL_HIGHPASSFILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									osc1.sendMsg ("/vcs", 3145729, 0.99999f);
									break;
								case 1:
								cubecube.Image("FULL",0,0,0,FULL_LOWPASSFILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									//Filter Frequency
									osc1.sendMsg ("/vcs", 3145729, 0.5f);
									break;
								case 2:
								cubecube.Image("FULL",0,0,0,FULL_FILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									//Filter Frequency
									osc1.sendMsg ("/vcs", 3145729, 0.0f);
									break;
							}
							break;
						case 4:
							switch (wrapper.mState) 
							{
								case 2:
								cubecube.Image("FULL",0,0,0,FULL_LFO2,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									//BPM + Modulation
									osc1.sendMsg ("/vcs", 3145735, 0.0f);
									osc1.sendMsg ("/vcs", 2, 0.0f);
									break;
								case 1:
								cubecube.Image("FULL",0,0,0,FULL_LFO1,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									osc1.sendMsg ("/vcs", 3145735, 0.25f);
									osc1.sendMsg ("/vcs", 2, 0.25f);
									//BPM + Modulation
									break;
								case 0:
								cubecube.Image("FULL",0,0,0,FULL_MODULATION,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									osc1.sendMsg ("/vcs", 3145735, 0.5f);
									osc1.sendMsg ("/vcs", 2, 0.5f);
									//BPM + Modulation
									break;
							}
							break;
						case 5:
							switch (wrapper.mState) 
							{
								case 0:
								cubecube.Image("FULL",0,0,0,FULL_SPECIAL,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									//Voir ce qui est choisi
									break;
								case 1:
								cubecube.Image("FULL",0,0,0,FULL_SPECIAL2,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
									//voir ce qui est choisi
									break;
							}
						break;
					}
				}
							else //we don't send to kyma but we change the picture on the screen
							{
								switch(wrapper.mNumber)
								{
									case 2:
										WithdrawSound();
										switch(wrapper.mState)
										{
											case 1:
													//WithdrawSound();
													cubecube.Image("FULL",0,0,0,FULL_LSOFF,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													break;
												
											case 0:
												/*	Log.Debug ("  sound");
													Log.Debug ("entrée dans le hautparleur");*/
												switch (sample) 
												{
													//LS sound
												case 1:
													cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													Log.Debug ("on envoie du son");
													osc1.sendMsg ("/vcs", 3145731, 0.9999f);
													System.Threading.Thread.Sleep(50);
													break;
												case 2:
													cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													osc1.sendMsg ("/vcs", 3145732, 0.9999f);
													System.Threading.Thread.Sleep(50);
													break;
												case 3:
													cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													osc1.sendMsg ("/vcs", 3145733, 0.9999f);
													System.Threading.Thread.Sleep(50);
													break;
												case 4:
													cubecube.Image("FULL",0,0,0,FULL_LSON,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													osc1.sendMsg ("/vcs", 3145734, 0.9999f);
													System.Threading.Thread.Sleep(50);
													break;
												}
												//osc1.sendMsg("/vcs",3145731,
												break;
											}
									break;

									case 3:
										switch(wrapper.mState)
										{
											case 0:
													cubecube.Image("FULL",0,0,0,FULL_HIGHPASSFILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													osc1.sendMsg("/vcs",3145729,0.0f);
													break;
											case 1:
													cubecube.Image("FULL",0,0,0,FULL_LOWPASSFILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													//Filter Frequency
													osc1.sendMsg("/vcs",3145729,0.0f);
													break;
											case 2:
													cubecube.Image("FULL",0,0,0,FULL_FILTER,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
													//Filter Frequency
													osc1.sendMsg("/vcs",3145729,0.0f);
													break;
										}
										break;

									case 4:
										switch(wrapper.mState)
										{
											case 0:
												cubecube.Image("FULL",0,0,0,FULL_MODULATION,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												//BPM + Modulation
												osc1.sendMsg("/vcs",3145735,0.0f);
												osc1.sendMsg("/vcs", 2 ,0.0f);
												break;
											case 1:
												cubecube.Image("FULL",0,0,0,FULL_LFO1,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												osc1.sendMsg("/vcs",3145735,0.0f);
												osc1.sendMsg("/vcs", 2 ,0.0f);
												//BPM + Modulation
												break;
											case 2:
												cubecube.Image("FULL",0,0,0,FULL_LFO2,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												osc1.sendMsg("/vcs",3145735,0.0f);
												osc1.sendMsg("/vcs", 2 ,0.0f);
												//BPM + Modulation
												break;
										}
										break;

									case 5:
										switch(wrapper.mState)
										{
											case 0:
												cubecube.Image("FULL",0,0,0,FULL_SPECIAL,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												break;
											case 1:
												cubecube.Image("FULL",0,0,0,FULL_SPECIAL2,Cube.SCREEN_WIDTH,Cube.SCREEN_HEIGHT,0,0);
												break;
										}
										break;
							}
						}		
					wrapper.mHaschanged=false;
					}
				}
			}
		public void CheckNeighbors ()
		{
		/*	Cube[] connected = CubeHelper.FindConnected (SOURCE);

			if (connected.Length > 1) {

				foreach (Cube cubecon in connected) {

					CubeWrapper wrapper = (CubeWrapper)cubecon.userData;

					switch (wrapper.mNumber) {
					case 2:
						lsi = 1;
						break;
					case 3:
						filteri = 1;
						break;
					case 4:
						lfoi = 1;
						break;
					case 5:
						speciali = 1;
						break;
					}
				}
			}*/
		}
		public void Reaction ()
		{
		/*	CubeWrapper wrapperLS = (CubeWrapper)LS.userData;
			CubeWrapper wrapperFILTER = (CubeWrapper)FILTER.userData;
			CubeWrapper wrapperLFO = (CubeWrapper)LFO.userData;
			CubeWrapper wrapperSPECIAL = (CubeWrapper)SPECIAL.userData;

			if (lsi == 1 && filteri == 1 && lfoi == 1 && speciali == 1) 
			{
				//son total avec tout, voir les états des différents éléments
				switch(wrapperLS.mState){
					//on regarde l'état du haut parleur 
				case 0:
					//haut parleur éteint
					break;
				case 1:
					//haut parleur allumé
					break;
				}
			}



			foreach (int cubei in cubesint) {
				

				if (lsi ==0){
					//pas de son car haut parleur non connecté (afficher les images (reprendre le code))	 
				}
				else {

					switch(wrapperLS.mState){
						//on regarde l'état du haut parleur 
					case 0:
						//haut parleur éteint
						break;
					case 1:
						//haut parleur allumé
						break;
					}
					if(filteri==0)
					{
					//pas de filtre branché
					}

						else 
						{
							switch(wrapperFILTER.mState){
								//on regarde l'état du filtre 
							case 0:
								//filtre de base (rien)
								break;
							case 1:
								//filtre passe bas
								break;
							
							case 2:
							//filtre passe haut
							break;
						}
					}
					if(lfoi==0){}
					if(speciali==0){}
				}
			}
		*/}
		public void WithdrawSound()
		{
			osc1.sendMsg ("/vcs", 3145731, 0.0f);
			System.Threading.Thread.Sleep(10); 
			osc1.sendMsg ("/vcs", 3145732, 0.0f);
			System.Threading.Thread.Sleep(10); 
			osc1.sendMsg ("/vcs", 3145733, 0.0f);
			System.Threading.Thread.Sleep(10); 
			osc1.sendMsg ("/vcs", 3145734, 0.0f);
			System.Threading.Thread.Sleep(10);
		}
	}
}
	