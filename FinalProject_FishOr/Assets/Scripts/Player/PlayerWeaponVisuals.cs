using UnityEngine;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [Header("Equipment View Models")]
    public GameObject rodView;
    public GameObject bucketView;
    public GameObject smgView;
    public GameObject grenadeView;
    public GameObject rpgView;
    public GameObject mysteryBoxView;

    [Header("Rod Cast Animation")]
    public float castForwardAngle = 45f;
    public float castDownAngle = 25f;
    public float castAnimSpeed = 12f;

    private Quaternion rodDefaultRotation;
    private bool rodDefaultSaved = false;

    private void Start()
    {
        SaveRodDefaultRotation();
        HideAll();
    }

    private void SaveRodDefaultRotation()
    {
        if (rodView != null && !rodDefaultSaved)
        {
            rodDefaultRotation = rodView.transform.localRotation;
            rodDefaultSaved = true;
        }
    }

    public void ShowEquipment(EquipmentType equipmentType)
    {
        HideAll();

        switch (equipmentType)
        {
            case EquipmentType.Rod:
                if (rodView != null)
                {
                    SaveRodDefaultRotation();
                    rodView.SetActive(true);
                    rodView.transform.localRotation = rodDefaultRotation;
                }
                break;

            case EquipmentType.Bucket:
                if (bucketView != null)
                    bucketView.SetActive(true);
                break;

            case EquipmentType.SMG:
                if (smgView != null)
                    smgView.SetActive(true);
                break;

            case EquipmentType.Grenade:
                if (grenadeView != null)
                    grenadeView.SetActive(true);
                break;

            case EquipmentType.RPG:
                if (rpgView != null)
                    rpgView.SetActive(true);
                break;

            case EquipmentType.MysteryBox:
                if (mysteryBoxView != null)
                    mysteryBoxView.SetActive(true);
                break;
        }
    }

    public void HideAll()
    {
        if (rodView != null)
            rodView.SetActive(false);

        if (bucketView != null)
            bucketView.SetActive(false);

        if (smgView != null)
            smgView.SetActive(false);

        if (grenadeView != null)
            grenadeView.SetActive(false);

        if (rpgView != null)
            rpgView.SetActive(false);

        if (mysteryBoxView != null)
            mysteryBoxView.SetActive(false);
    }

    public void PlayRodCastAnimation()
    {
        if (rodView == null)
            return;

        StopAllCoroutines();
        StartCoroutine(RodCastRoutine());
    }

    private System.Collections.IEnumerator RodCastRoutine()
    {
        SaveRodDefaultRotation();

        rodView.SetActive(true);

        Quaternion castRotation = rodDefaultRotation * Quaternion.Euler(castDownAngle, 0f, castForwardAngle);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * castAnimSpeed;
            rodView.transform.localRotation = Quaternion.Slerp(rodDefaultRotation, castRotation, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * castAnimSpeed;
            rodView.transform.localRotation = Quaternion.Slerp(castRotation, rodDefaultRotation, t);
            yield return null;
        }

        rodView.transform.localRotation = rodDefaultRotation;
    }
}