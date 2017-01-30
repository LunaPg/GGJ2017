using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParleyInvironmentInfoDictionary : MonoBehaviour, ParleyEnviromentInfo {

	Dictionary<string, object> info = new Dictionary<string, object>();

	void Start () {
		Parley.GetInstance().SetParleyEnviromentInfo(this);
	}

	public object GetEnviromentInfo(string key) {
		if (info.ContainsKey(key)){
			return info[key];
		}
		return null;
	}

	public void SetEnviromentInfo(string key,object value) {
		if (info.ContainsKey(key)) {
			info.Remove(key);
		}
		info.Add(key, value);
	}
}
