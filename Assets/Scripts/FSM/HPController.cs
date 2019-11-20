using UnityEngine;

public class HPController : MonoBehaviour{



	private float hp;
	[SerializeField]
	private float maxHp;

	public float HP{
		get{ return hp; }
	}

	public float MaxHp{
		get{ return maxHp; }
	}

	private void Awake(){
		hp = maxHp;
	}

	public void DealDamage(float damage){
		hp -= damage;
	}

	public void Heal(float healing){
		hp += healing;
		if(hp >= maxHp){
			hp = maxHp;
		}
	}


}
