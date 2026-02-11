using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance;

    [SerializeField] private WeaponController controller;

    private void Awake()
    {
        Instance = this;
    }



}
