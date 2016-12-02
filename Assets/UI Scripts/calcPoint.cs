using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class calcPoint : MonoBehaviour {

	public GameObject roomFloor;
	public GameObject dropdownLights;

	public void buttonClick(){

		Dictionary<string, double> calcPoints = new Dictionary<string, double> ();
		List<double> calculations = new List<double> ();

		Vector3 point;
		float angle;
		float distance;
		Vector3 light;
		int multiplier = 5;
		double returnValue;
		double testLuminInten = 35.2;
		string thePoint;
		float pointsPerSQFT;
		float pointsPerSQFTX;
		float pointsPerSQFTZ;
		float calcPointHeight;
		float spacing;
		float pointHeight;
		double total = 0;


		//needed info from the room is the floor plane name and the lights name


		//User inputed values
		calcPointHeight = 2F;
		pointsPerSQFT = 2F;


		//This is convert into feet from meters
		pointsPerSQFT *= 3;


		//get the coordinates of the center position of the plane
		Vector3 groundCenter = roomFloor.transform.position;
		//Vector3 groundCenter = GameObject.Find ("groundPlane").transform.position;

		//get the dimensions of the plane
		Vector3 groundScale = roomFloor.transform.localScale;
		//Vector3 groundScale = GameObject.Find ("groundPlane").transform.localScale;

		//get the x & z values of the dimensions
		float scaleX = groundScale.x;
		float scaleZ = groundScale.z;

		//get the x, y, & z coordinates of the planes center
		float x = groundCenter.x;
		float y = groundCenter.y;
		float z = groundCenter.z;


		//print ("plane x:" + x);
		//print ("plane z:" + z);

		//get the length and width of the plane
		float xLength = scaleX * 10;
		float zLength = scaleZ * 10;

		//get the corner x & z coordinates of the plane
		float ballX = (scaleX * 5) + x;
		float ballZ = (scaleZ * 5) + z;

		pointsPerSQFTX = pointsPerSQFT * scaleX;
		pointsPerSQFTZ = pointsPerSQFT * scaleZ;

		float spacingX = xLength / pointsPerSQFTX;
		float spacingZ = zLength / pointsPerSQFTZ;

		//print ("Spacint x val: " + spacingX);
		//print ("Spacint z val: " + spacingZ);


		ballX -= xLength;
		ballZ -= zLength;

		float initialStartX = spacingX / 2;
		float initialStartZ = spacingZ / 2;

		ballX += initialStartX;
		ballZ += initialStartZ;


		float stopX = (scaleX * 5) + x;
		float stopZ = (scaleZ * 5) + z;

		//stopX += xLength;
		//stopZ += zLength;

		print ("z ball: " + ballZ);
		print ("stopping x: " + stopX);
		print ("stopping z: " + stopZ);



		calcPointHeight *= .333F;
		pointHeight = (y + (calcPointHeight));

		int count = 0;

		//these two loops position all of the points on the plane
		while (ballX < stopX) {

			while (ballZ < stopZ) {

				GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				sphere.name = "calcPoint" + count;
				sphere.transform.localScale = new Vector3 (.2F, .2F, .2F);
				sphere.transform.position = new Vector3 (ballX, pointHeight, ballZ);
				sphere.GetComponent<SphereCollider> ().enabled = false;
				ballZ += spacingZ;
				//print ("ballZ val: " + ballZ);
				count++;

			}

			ballX += spacingX;
			ballZ = (scaleZ * 5) + z;
			ballZ -= zLength;
			ballZ += initialStartZ;

		}


		print ("Total calc points: " + count);


		IESSelector lights = (IESSelector)dropdownLights.GetComponent (typeof(IESSelector));
		ArrayList lightList = lights.getLightList ();
		for (int i = 0; i < count; i++) {
			point = GameObject.Find ("calcPoint" + i).transform.position;
			foreach (GameObject l in lightList) {
				light = l.transform.position;
				angle = Vector3.Angle (light, point);
				distance = Vector3.Distance (light, point);
				distance = distance - 3;
				//print ("distance for point :" + count + " is: " + distance);

				//this commented part is to account for distance
				if (distance < 4) {
					returnValue = doMath (distance, testLuminInten, angle);
					calculations.Add (returnValue);
				}
			}

			//add the value into the dictionary
			//
			thePoint = "calcPoint" + i;
			//print ("The size of this list is: " + calculations.Count);
			foreach (double l in calculations) {

				total = total + l;

			}
			total = (total / calculations.Count);
			print (thePoint + ": " + total);

			calcPoints.Add(thePoint, total);
			total = 0;
			calculations.Clear ();

		}




	}


	public double doMath(float distance, double lumin, float angle){
		/*
		** equation is (I/d^2)*cos(theta)
		** d = distance from the light to the point
		** theta is the angle given by creating a perpendicular line on the point and getting th angle between that and distance vector
		** I = illuminous intensity in the direction of the point
		*/

		double point = ((lumin * Mathf.Cos(angle)/(Mathf.Pow(distance, 2))));

		if (point <= 0) {
			point = point * -1;
		}

		//print ("point: " + point);

		return point;

	}

}