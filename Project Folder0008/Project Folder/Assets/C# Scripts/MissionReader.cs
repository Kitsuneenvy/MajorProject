using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MissionReader : MonoBehaviour {
	
	//A star gameobject
	AstarPath aStarGrid;
	
	//holds current position of the astar grid
	Vector3 currentPosition; 
	
	//whether new mission
	public bool newMission = true;
	public bool recheck = false;
	
	public int currentMission = 0;
	
	//which mission to be created
	public bool tutorial = false;
	public bool mission1 = false;
	public bool mission2 = false;
	public bool mission3 = false;
	public bool mission4 = false;
	public bool mission5 = false;
	public bool flipped = false;
	public bool rotate = false;
	public string objective = "";
	
	
	string autoSavedMission = "";
	int rotationY = 0; //how much to rotate enemies by
	
	//has file been read and positions assigned
	public bool layoutCompleted = false;
	
	//name of text file containing mission data
	string textFileName = "";
	string autoSave = "";
	
	//holds lines from text file
	List<string> fileLines = new List<string>();
	
	//holds unit variables
	List<int> unitInfo = new List<int>();
	
	List<string> nameList = new List<string>();
	List<string> bowlderNames = new List<string>();
	List<string> frierNames = new List<string>();
	List<string> ladleNames = new List<string>();
	List<string> potterNames = new List<string>();
	List<string> mowerNames = new List<string>();
	List<string> floristNames = new List<string>();
	List<string> prunerNames = new List<string>();
	
	//holds all units
	public List<GameObject> allUnits = new List<GameObject>();
	
	public List<GameObject> enemyUnits = new List<GameObject>();
	
	public List<GameObject> optionalTiles = new List<GameObject>();
	
	public List<GameObject> flowerUnits = new List<GameObject>();
	
	List<GameObject> flowerAdjacentTiles = new List<GameObject>();
	
	//main camera object
	public GameObject mainCamera;
	GameObject saveData;
	
	public GameObject flower;
	
	int counter = 0;
	int fileLineCounter = 0;
	//Holds Units
	GameObject tempUnit = null;
	
	//holds temporary vector of gridSquare for unit to be placed on
	Vector3 tempPosition;
	
	//checkmark uisprite
	public UISprite checkmark;
	
	public bool statsIncreased = false;
	
	ParticleSystem particleOnGrid;
	
	public Material particleMaterial;
	
	FileInfo savFile;
	
	// Use this for initialization
	void Start () {
		readNameList();
		aStarGrid = GameObject.Find("A*").GetComponent<AstarPath>();
		saveData = GameObject.Find("SaveData");
		checkmark = GameObject.Find("ObjectivesCheckmark").GetComponent<UISprite>();
		
		if(Application.loadedLevelName == "Tutorial"){
			tutorial = true;
			mission1 = false;
			autoSavedMission = saveData.GetComponent<StoreData>().returnAutoSaveName();
			savFile =  new FileInfo(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav");
		}
		else
		{
			if(saveData.GetComponent<StoreData>().ReturnMission() != "" && saveData.GetComponent<StoreData>().ReturnMission() != null)
			{
				autoSavedMission = saveData.GetComponent<StoreData>().returnAutoSaveName();
				//get mission, set whatever mission it contains to true
				string temp = saveData.GetComponent<StoreData>().ReturnMission();
				//temporarily
				if(temp == "Mission1")
				{
					currentMission = 1;
					mission1 = true;
				}
				else if (temp == "Mission2")
				{
					currentMission = 2;
					mission2 = true;
				}
				else if(temp == "Mission3")
				{
					currentMission = 3;
					mission3 = true;
				}
				else if (temp == "Mission4")
				{
					currentMission = 4;
					mission4 = true;
				}
				else if (temp == "Mission5")
				{
					currentMission = 5;
					mission5 = true;
				}
				
			}
			else
			{
				
				autoSavedMission = saveData.GetComponent<StoreData>().returnAutoSaveName();
				mission1 = true;
			}
			savFile =  new FileInfo(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav");
//			GameObject enemyTurnText = GameObject.Instantiate(Resources.Load("DamageText")) as GameObject;
//				enemyTurnText.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
//				enemyTurnText.transform.localPosition = new Vector3(-4,0,9);
//				enemyTurnText.GetComponent<TextMesh>().text = savFile.ToString();
			newMission = true;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if(recheck == true){
			if(newMission == true)
			{	
				optionalTiles.Clear();
				flowerUnits.Clear();
				newMission = false;
				layoutCompleted = false;
				objective = "";
				readNameList();
				if(mission1 == true)
				{
					currentMission = 1;
					objective = "Kill All Enemies";
				}
				else if (mission2 == true)
				{	
					currentMission = 2;
					objective = "Reach one tile \n right of fence";
					aStarGrid.astarData.RemoveGraph(aStarGrid.astarData.active.graphs[0]);
					aStarGrid.astarData.AddGraph("GridGraph");
					aStarGrid.Scan();
					aStarGrid.astarData.gridGraph.nodeSize = 5;
				}
				else if(mission3 == true)
				{
					currentMission = 3;
					objective = "Rout Enemies";
					aStarGrid.astarData.RemoveGraph(aStarGrid.astarData.active.graphs[0]);
					aStarGrid.astarData.AddGraph("GridGraph");
					aStarGrid.Scan();
					aStarGrid.astarData.gridGraph.nodeSize = 5;
	
				}
				else if (mission4 == true)
				{
					currentMission = 4;
					objective = "Defend chef";
					aStarGrid.astarData.RemoveGraph(aStarGrid.astarData.active.graphs[0]);
					aStarGrid.astarData.AddGraph("GridGraph");
					aStarGrid.Scan();
					aStarGrid.astarData.gridGraph.nodeSize = 5;
				}
				else if (mission5 == true)
				{
					currentMission = 5;
					objective = "Stop the florist \n from escaping";
					aStarGrid.astarData.RemoveGraph(aStarGrid.astarData.active.graphs[0]);
					aStarGrid.astarData.AddGraph("GridGraph");
					aStarGrid.Scan();
					aStarGrid.astarData.gridGraph.nodeSize = 5;
				}
				Read();
				
			}
		} else {
			recheck = true;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{	
			newMission = true;
			if(counter == 1)
			{
				mission2 = true;
			}
			if(counter == 2)
			{
				
				mission3 = true;
			}
			if(counter == 3)
			{
				
				mission4 = true;
			}
			if(counter == 4)
			{
				
				mission5 = true;
			}
			layoutCompleted = false;
		}
	}
	
	void Read()
	{
		if(allUnits.Count != 0)
		{
			foreach(GameObject unit in allUnits)
			{
				Destroy(unit);
			}
			allUnits.Clear();
		}
		layoutCompleted = false;
		//mission number
		if(mission1)
		{
			//location of mission data
			textFileName = "Assets/MissionFiles/Mission1.mis";
			autoSave ="Assets/MissionFiles/Mission1.mis";
			//set new mission to false
			mission1 = false;
			currentMission = 1;
			newMission = false;
			if(savFile.Exists == true)
			{
				List<string> holdLines = new List<string>();
				fileLineCounter = 0;
				foreach(string line in File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
				{
					
					holdLines.Add(line);
					fileLineCounter++;
				}
				if(!holdLines.Contains("Mission1"))
				{
					using(StreamWriter sw = savFile.AppendText())
					{
						sw.WriteLine("Mission1");
					}
				}
			}
			counter = 1;
		}
		if(mission2)
		{
			textFileName = "Assets/MissionFiles/Mission2.mis";
			autoSave = "Assets/MissionFiles/Mission2.mis";
			currentMission = 2;
			newMission = false;
			if(savFile.Exists == true)
			{
				List<string> holdLines = new List<string>();
				fileLineCounter = 0;
				foreach(string line in File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
				{
					holdLines.Add(line);
					fileLineCounter++;
					if(fileLineCounter == 2)
					{
						fileLineCounter = 0;
						foreach(string storedLine in holdLines)
						{
							if(fileLineCounter == 0)
							{
								using (StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
								{
									sw.WriteLine(storedLine);
								}
							}
							if(fileLineCounter == 1)
							{
								using(StreamWriter sw = savFile.AppendText())
								{
									sw.WriteLine("Mission2");
								}
							}
							fileLineCounter++;
						}
					}
				}
			}
			counter = 2;
		}
		if(mission3)
		{
			textFileName = "Assets/MissionFiles/Mission3.mis";
			autoSave = "Assets/MissionFiles/Mission3.mis";
			//set new mission to false
			newMission = false;
			if(savFile.Exists == true)
			{
				List<string> holdLines = new List<string>();
				fileLineCounter = 0;
				
				foreach(string line in File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
				{
					holdLines.Add(line);
					fileLineCounter++;
					if(fileLineCounter == 2)
					{
						fileLineCounter = 0;
						foreach(string storedLine in holdLines)
						{
							if(fileLineCounter == 0)
							{
								using (StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
								{
									sw.WriteLine(storedLine);
								}
							}
							if(fileLineCounter == 1)
							{
								using(StreamWriter sw = savFile.AppendText())
								{
									sw.WriteLine("Mission3");
								}
							}
							fileLineCounter++;
						}
					}
				}
			}
			counter = 3;
		}
		if(mission4)
		{
			textFileName = "Assets/MissionFiles/Mission4.mis";
			autoSave = "Assets/MissionFiles/Mission4.mis";
			//set new mission to false
			newMission = false;
			if(savFile.Exists == true)
			{
				List<string> holdLines = new List<string>();
				fileLineCounter = 0;
				
				foreach(string line in File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
				{
					holdLines.Add(line);
					fileLineCounter++;
					if(fileLineCounter == 2)
					{
						fileLineCounter = 0;
						foreach(string storedLine in holdLines)
						{
							if(fileLineCounter == 0)
							{
								using (StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
								{
									sw.WriteLine(storedLine);
								}
							}
							if(fileLineCounter == 1)
							{
								using(StreamWriter sw = savFile.AppendText())
								{
									sw.WriteLine("Mission4");
								}
							}
							fileLineCounter++;
						}
					}
				}
			}
			counter = 4;
		}
		if(mission5)
		{
			textFileName = "Assets/MissionFiles/Mission5.mis";
			autoSave = "Assets/MissionFiles/Mission5.mis";
			//set new mission to false
			newMission = false;
			if(savFile.Exists == true)
			{
				List<string> holdLines = new List<string>();
				fileLineCounter = 0;
				
				foreach(string line in File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
				{
					holdLines.Add(line);
					fileLineCounter++;
					if(fileLineCounter == 2)
					{
						fileLineCounter = 0;
						foreach(string storedLine in holdLines)
						{
							if(fileLineCounter == 0)
							{
								using (StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))
								{
									sw.WriteLine(storedLine);
								}
							}
							if(fileLineCounter == 1)
							{
								using(StreamWriter sw = savFile.AppendText())
								{
									sw.WriteLine("Mission5");
								}
							}
							fileLineCounter++;
						}
					}
				}
			}
		}
		if(tutorial){
			textFileName = "Assets/MissionFiles/Tutorial.mis";
		}
		
		
		int lineCounter = 0;
		
		//set current position
		currentPosition = aStarGrid.transform.position;
		
		//go through and add all lines from file to list
		foreach(string line in File.ReadAllLines(textFileName))
		{
			fileLines.Add(line);
			lineCounter++;
		}
		for(int i = 0; i<fileLines.Count; i++)
		{
			if(fileLines[i].Contains("-G"))
			{
				int counter = i;
				
				//set positions x,y,z
				currentPosition.x = int.Parse(fileLines[counter+1]);
				currentPosition.y = int.Parse(fileLines[counter+2]);
				currentPosition.z = int.Parse(fileLines[counter+3]);
				//move graph
				aStarGrid.astarData.gridGraph.center = currentPosition;
				//move collider
				aStarGrid.transform.position = currentPosition;
				//set node width and depth
				aStarGrid.astarData.gridGraph.width = int.Parse(fileLines[counter+4]);
				aStarGrid.astarData.gridGraph.depth = int.Parse(fileLines[counter+5]);
				if(mission2){
					mission2 = false;
				}
				if(mission3)
				{
					aStarGrid.astarData.gridGraph.rotation.y = 13;
					currentMission = 3;
					mission3 = false;
					rotate = true;
				}
				if(mission4)
				{
					aStarGrid.astarData.gridGraph.rotation.y = 325;
					currentMission = 4;
					mission4 = false;
					flipped = true;
				}
				if(mission5)
				{
					aStarGrid.astarData.gridGraph.rotation.y = 360;
					currentMission = 5;
					mission5 = false;
				}
				aStarGrid.astarData.gridGraph.UpdateSizeFromWidthDepth();
				aStarGrid.astarData.gridGraph.Scan();
			}
			if(fileLines[i].Contains("-Pre="))
			{
				//pre battle text
				string pre = fileLines[i].ToString();
				pre.Substring(5,pre.Length-5);
			}
			if(fileLines[i].Contains("-Post="))
			{
				//post battle text
				string post = fileLines[i].ToString();
				post.Substring(6,post.Length-6);
			}
			
			}
		//layout completed
			layoutCompleted = true;
//			if(mainCamera.GetComponent<CameraMovement>().noMove==false&&GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().cinematicComplete == true){
//				mainCamera.GetComponent<CameraMovement>().moveCamera = true;
//			}
				
		}
	
	//returns newMission true/false
	public bool returnNewMission()
	{
		return newMission;
	}
	
	//returns layoutCompleted true/false
	public bool returnLayoutCompleted()
	{
		return layoutCompleted;
	}
	
	void CalculateGridPosition()
	{
		if(tempUnit!=null){
			//total X * (Yc - 1) + Xc
			int gridSquareNumber = aStarGrid.astarData.gridGraph.width * (unitInfo[2] - 1) + unitInfo[1];
			tempPosition = tempUnit.transform.position;
			tempPosition.x = GameObject.Find("GridSquare(Clone)" + gridSquareNumber.ToString()).transform.position.x;
			tempPosition.z = GameObject.Find("GridSquare(Clone)" + gridSquareNumber.ToString()).transform.position.z;
			tempPosition.y = 60;
			if(tempUnit.name.Contains("Flower"))
			{
				tempPosition.y = 61.5f;
			}
			if(tutorial && tempUnit.tag == "Enemy")
			{
				tutorial = false;
				optionalTiles.Add(GameObject.Find("GridSquare(Clone)" + (gridSquareNumber-1).ToString()));
				optionalTiles.Add(GameObject.Find("GridSquare(Clone)" + (gridSquareNumber+ aStarGrid.astarData.gridGraph.width).ToString()));
				optionalTiles.Add(GameObject.Find("GridSquare(Clone)" + (gridSquareNumber+1).ToString()));
			}
			tempUnit.transform.position = tempPosition;
			
			//GameObject.Find("GridSquare(Clone)" + gridSquareNumber.ToString()).GetComponent<Grid>().OnTriggerEnter(tempUnit.collider);
			if(!tempUnit.name.Contains("Flower"))
			{
				tempUnit.GetComponent<UnitGenerics>().Initialise(unitInfo[0]);
			} else {
				tempUnit.GetComponent<UnitGenerics>().Initialise(4);
			}
			tempUnit.GetComponent<UnitGenerics>().setGrid(GameObject.Find("GridSquare(Clone)" + gridSquareNumber).GetComponent<Grid>());
			tempUnit.GetComponent<UnitGenerics>().onGrid.GetComponent<Grid>().heldUnit = tempUnit;
			
			//clear unit info
			unitInfo.Clear();
			tempUnit = null;
		}
	}
	
	public void PositionUnits()
	{
		//iterator for unit type, positions and allegiance
		int splitLineIterator = 0;
		
		for(int i = 0; i<fileLines.Count; i++)
		{
			if(fileLines[i].Contains("+U"))
			{
				//set unit positions
				for(int j = i+1; j <fileLines.Count;j++)
				{
					string[] splitLine = fileLines[j].Split(new string[] {"(",",",")"},System.StringSplitOptions.RemoveEmptyEntries);
					
					foreach(string line in splitLine)
					{
						if(!line.Contains("(") && !line.Contains(",") && !line.Contains(")"))
						{
							splitLineIterator++;
							
							unitInfo.Add(int.Parse(line));
							
							if(splitLineIterator == 4)
							{
								//if unit friendly
								if(unitInfo[3] == 0)
								{
									if(!flipped)
									{
										rotationY = 0;
									}
									else
									{
										rotationY = 180;
									}
									if(unitInfo[0] == 0)//speed
									{
										int charNameNumber = Random.Range(0,frierNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Frier")) as GameObject;
										tempUnit.tag = "PlayerUnit";
										tempUnit.name = frierNames[charNameNumber];
										frierNames.Remove(frierNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y+rotationY,0);
									}
									else if(unitInfo[0] == 1)//attack
									{
										int charNameNumber = Random.Range(0,ladleNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Ladlewight")) as GameObject;
										tempUnit.tag = "PlayerUnit";
										tempUnit.name = ladleNames[charNameNumber];
										ladleNames.Remove(ladleNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y+rotationY,0);
									}
									else if(unitInfo[0] == 2)//defence
									{
										int charNameNumber = Random.Range(0,bowlderNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Bowlder")) as GameObject;
										tempUnit.tag = "PlayerUnit";
										tempUnit.name = bowlderNames[charNameNumber];
										bowlderNames.Remove(bowlderNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y+rotationY,0);
									}
									else if(unitInfo[0] == 3)//healer
									{
										tempUnit = GameObject.Instantiate(Resources.Load("Chef")) as GameObject;
										tempUnit.tag = "PlayerUnit";
										tempUnit.name = (File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+autoSavedMission+".sav"))[0];
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y+rotationY,0);
									}
									
								}
								//if unit enemy
								else if(unitInfo[3]== 1)
								{
									if(!flipped)
									{
										rotationY = 180;
									}
									else
									{
										rotationY = 0;
									}
									if(unitInfo[0] == 0)//speed
									{
										int charNameNumber = Random.Range(0,mowerNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Mower")) as GameObject;
										tempUnit.tag = "Enemy";
										tempUnit.name = mowerNames[charNameNumber];
										mowerNames.Remove(mowerNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
									}
									else if(unitInfo[0] == 1)//attack
									{
										int charNameNumber = Random.Range(0,prunerNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Pruner")) as GameObject;
										tempUnit.tag = "Enemy";
										tempUnit.name = prunerNames[charNameNumber];
										prunerNames.Remove(prunerNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
									}
									else if(unitInfo[0] == 2)//defence
									{
										int charNameNumber = Random.Range(0,potterNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Potter")) as GameObject;
										tempUnit.tag = "Enemy";
										tempUnit.name = potterNames[charNameNumber];
										potterNames.Remove(potterNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
									}
									else if(unitInfo[0] == 3)//healer
									{
										int charNameNumber = Random.Range(0,floristNames.Count-1);
										tempUnit = GameObject.Instantiate(Resources.Load("Florist")) as GameObject;
										tempUnit.tag = "Enemy";
										tempUnit.name = floristNames[charNameNumber];
										floristNames.Remove(floristNames[charNameNumber]);
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
									}
									else if(unitInfo[0] == 4)//flower
									{
										tempUnit = GameObject.Instantiate(flower) as GameObject;
										tempUnit.name = "Flower";
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
									}
								}
								
								//if unit object
								else if(unitInfo[3] == 2)
								{
									switch(unitInfo[0])
									{
									case 5:
									{
										tempUnit = GameObject.Instantiate(Resources.Load("Fence")) as GameObject;
										tempUnit.tag = "Object";
										tempUnit.name = "Fence";
										if(currentMission == 2)
										{
											tempUnit.transform.rotation = Quaternion.Euler(0,90,0);
										}
										else
										{
											tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
										}
										break;
									}
									case 6:
									{
										tempUnit = GameObject.Instantiate(Resources.Load("Stand")) as GameObject;
										tempUnit.tag = "Object";
										tempUnit.name = "Stand";
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
										break;
									}
									case 7:
									{
										tempUnit = GameObject.Instantiate(Resources.Load("Pot")) as GameObject;
										tempUnit.tag = "Object";
										tempUnit.name = "Pot";
										tempUnit.transform.rotation = Quaternion.Euler(0,aStarGrid.astarData.gridGraph.rotation.y +rotationY,0);
										break;
									}
									default:
									{
										break;
									}
									}
								}
								if(unitInfo[0] != 4){
									tempUnit.AddComponent("EllipsoidParticleEmitter");
									tempUnit.AddComponent("ParticleAnimator");
									tempUnit.AddComponent("ParticleRenderer");
									tempUnit.GetComponent("EllipsoidParticleEmitter").particleEmitter.maxEmission = 0;
									tempUnit.GetComponent<ParticleRenderer>().material = particleMaterial;
								}
								allUnits.Add(tempUnit);
								//position the unit
								CalculateGridPosition();
								//reset iterator
								splitLineIterator = 0;
								unitInfo.Clear();
							}
						}
						
					}
				}
				//clear list
				fileLines.Clear();
			}
		}
	foreach(GameObject unit in allUnits){
			if(unit.tag == "Enemy" && !unit.name.Contains("Flower"))
			{
				enemyUnits.Add(unit);
			}
			if(unit.name.Contains("Flower"))
			{
				flowerUnits.Add(unit);
			}
			unit.GetComponent<UnitGenerics>().setGrid(unit.GetComponent<UnitGenerics>().onGrid);
			
		}
		aStarGrid.Scan();
		if(flowerUnits.Count > 0)
		{
			foreach( GameObject flowerUnit in flowerUnits)
			{
				flowerAdjacentTiles = flowerUnit.GetComponent<UnitGenerics>().checkAdjacentGrids(flowerUnit.GetComponent<UnitGenerics>().onGrid.gameObject);
				
				foreach(GameObject tile in flowerAdjacentTiles)
				{
					tile.AddComponent("MeshParticleEmitter");
					tile.AddComponent<ParticleRenderer>();
					tile.AddComponent<ParticleAnimator>();
					tile.particleEmitter.maxSize = 1;
					tile.particleEmitter.worldVelocity = new Vector3(0,1,0);
					tile.GetComponent<ParticleRenderer>().material = particleMaterial;
					Color[] animatorColours = tile.GetComponent<ParticleAnimator>().colorAnimation;
					animatorColours[0] = new Color(0f,0f,0f,1f);
					animatorColours[1] = new Color(1f,0f,0f,1f);
					animatorColours[2] = new Color(1f,0.075f,0.075f,1f);
					animatorColours[3] = new Color(1f,0.15f,0.15f,1f);
					animatorColours[4] = new Color(0.6f,0.5f,0.3f,1f);
					tile.GetComponent<ParticleAnimator>().colorAnimation = animatorColours;

					
					Grid gridObject = tile.GetComponent<Grid>();
					if(gridObject.heldUnit != null && gridObject.heldUnit.tag != "PlayerUnit" && (!gridObject.heldUnit.name.Contains("Flower") || !gridObject.heldUnit.name.Contains("Florist")))
					{
						gridObject.heldUnit.GetComponent<UnitGenerics>().statsIncreased = true;
						gridObject.heldUnit.GetComponent<UnitGenerics>().attack += 10;
					}
				}
			}
			
		}
	}
	void readNameList(){
		nameList.Clear();
		bowlderNames.Clear();
		frierNames.Clear();
		floristNames.Clear();
		mowerNames.Clear();
		ladleNames.Clear();
		prunerNames.Clear();
		potterNames.Clear();
		foreach(string line in File.ReadAllLines("Assets/MissionFiles/NameList.txt")){
			nameList.Add(line);
		}
		foreach(string line in nameList){
			if(line.StartsWith("<Bowl")){
				for(int i = 1; i<5; i++){
					bowlderNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Fri")){
				for(int i = 1; i<5; i++){
					frierNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Flor")){
				for(int i = 1; i<5; i++){
					floristNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Mow")){
				for(int i = 1; i<5; i++){
					mowerNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Ladl")){
				for(int i = 1; i<6; i++){
					ladleNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Prun")){
				for(int i = 1; i<5; i++){
					prunerNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
			if(line.StartsWith("<Pott")){
				for(int i = 1; i<5; i++){
					potterNames.Add(nameList[nameList.IndexOf(line)+i]);
				}
			}
		}
	}
	public void whatTheA(){
			newMission = true;
			if(counter == 1)
			{
				mission2 = true;
			}
			if(counter == 2)
			{
				
				mission3 = true;
			}
			if(counter == 3)
			{
				
				mission4 = true;
			}
			if(counter == 4)
			{
				
				mission5 = true;
			}
			layoutCompleted = false;
		}
}
