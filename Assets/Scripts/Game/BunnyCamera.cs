using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class BunnyCamera : MonoBehaviour
    {
        [SerializeField, Range(3, 10)] private float cameraZoom = 5.0f;
        [SerializeField] private float zoomSpeed = 1.0f;
        [SerializeField] private float rotationSpeed = 1.0f;
        [FormerlySerializedAs("moveSpeed")] [SerializeField] private float followMoveSpeed = 1.0f;
        [SerializeField] private float freeMoveSpeed = 1.0f;

        public RotationPreset currentRotationPreset = RotationPreset.HundredEighty;
        
        private Grid.Item   m_cameraTarget;

        private Quaternion m_targetRotation;

        private Vector3 wMoveVector = new Vector3(0f, 0f, 1f);
        private Vector3 aMoveVector = new Vector3(-1f, 0f, 0f);
        private Vector3 sMoveVector = new Vector3(0f, 0f, -1f);
        private Vector3 dMoveVector = new Vector3(1f, 0f, 0f);

        void Update()
        {
            /* grab any item to follow
            if (m_cameraTarget == null && 
                Grid.Instance != null)
            {
                m_cameraTarget = Grid.Instance.transform.GetComponentInChildren<Grid.Item>();
            }*/

            // follow that item!
            if (m_cameraTarget != null)
            {
                StatManager.Instance.SelectedStatObject.SetActive(true);

                ItemInfo selectedItemInfo = m_cameraTarget.GetComponentInParent<ItemInfo>();
                StatManager.Instance.statObjectImage.sprite = selectedItemInfo.itemSprite;
                StatManager.Instance.statObjectName.text = selectedItemInfo.itemName;

                Bunny selectedBunny = null;
                Wolf selectedWolf = null;

                selectedBunny = m_cameraTarget.GetComponent<Bunny>();
                selectedWolf = m_cameraTarget.GetComponent<Wolf>();

                if (selectedBunny != null)
                {
                    StatManager.Instance.statObjectHunger.text = "Hunger: " + selectedBunny.Hunger;
                    StatManager.Instance.statObjectLustMeter.value = selectedBunny.MatingCooldown / 30;
                    StatManager.Instance.statObjectCurrentState.text = "Current State: " + selectedBunny.Machine.CurrentState.name;
                }
                if (selectedWolf != null)
                {
                    StatManager.Instance.statObjectHunger.text = "Hunger: " + selectedWolf.Hunger;
                    StatManager.Instance.statObjectLustMeter.value = selectedWolf.MatingCooldown / 90;
                    StatManager.Instance.statObjectCurrentState.text = "Current State: " + selectedWolf.Machine.CurrentState.name;
                }
                
                Vector3 vTarget = m_cameraTarget.transform.position - transform.forward * cameraZoom;
                transform.position += (vTarget - transform.position) * Time.deltaTime * followMoveSpeed;
            }
            else
            {
                StatManager.Instance.SelectedStatObject.SetActive(false);
                cameraZoom = 10f;
                transform.position = new Vector3(transform.position.x, 8.66f, transform.position.z);
                
                if(Input.GetKey(KeyCode.W))
                {
                    transform.position += wMoveVector * Time.deltaTime * freeMoveSpeed;
                }
                if(Input.GetKey(KeyCode.S))
                {
                    transform.position += sMoveVector * Time.deltaTime * freeMoveSpeed;
                }
                if(Input.GetKey(KeyCode.A))
                {
                    transform.position += aMoveVector * Time.deltaTime * freeMoveSpeed;
                }
                if(Input.GetKey(KeyCode.D))
                {
                    transform.position += dMoveVector * Time.deltaTime * freeMoveSpeed;
                }
            }

            if (Input.mouseScrollDelta.y == 1 && m_cameraTarget != null)
            {
                cameraZoom -= Time.deltaTime * zoomSpeed;
                cameraZoom = Mathf.Max(cameraZoom, 3);
            }
            if (Input.mouseScrollDelta.y == -1 && m_cameraTarget != null)
            {
                cameraZoom += Time.deltaTime * zoomSpeed;
                cameraZoom = Mathf.Min(cameraZoom, 10);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (currentRotationPreset)
                {
                    case RotationPreset.Zero:
                        m_targetRotation = Quaternion.Euler(60, 90, 0);
                        
                        wMoveVector = new Vector3(1f, 0f, 0f);
                        aMoveVector = new Vector3(0, 0f, 1f);
                        sMoveVector = new Vector3(-1f, 0f, 0f);
                        dMoveVector = new Vector3(0, 0f, -1f);
                        
                        currentRotationPreset = RotationPreset.Ninty;
                        break;
                    
                    case RotationPreset.Ninty:
                        m_targetRotation = Quaternion.Euler(60, 180, 0);
                        
                        wMoveVector = new Vector3(0f, 0f, -1f);
                        aMoveVector = new Vector3(1f, 0f, 0f);
                        sMoveVector = new Vector3(0f, 0f, 1f);
                        dMoveVector = new Vector3(-1f, 0f, 0f);
                        
                        currentRotationPreset = RotationPreset.HundredEighty;
                        break;
                    
                    case RotationPreset.HundredEighty:
                        m_targetRotation = Quaternion.Euler(60, 270, 0);
                        
                        wMoveVector = new Vector3(-1f, 0f, 0f);
                        aMoveVector = new Vector3(0, 0f, -1f);
                        sMoveVector = new Vector3(1f, 0f, 0f);
                        dMoveVector = new Vector3(0, 0f, 1f);
                        
                        currentRotationPreset = RotationPreset.TwoHundredSeventy;
                        break;
                    
                    case RotationPreset.TwoHundredSeventy:
                        m_targetRotation = Quaternion.Euler(60, 0, 0);
                        
                        wMoveVector = new Vector3(0f, 0f, 1f);
                        aMoveVector = new Vector3(-1f, 0f, 0f);
                        sMoveVector = new Vector3(0f, 0f, -1f);
                        dMoveVector = new Vector3(1f, 0f, 0f);
                        
                        currentRotationPreset = RotationPreset.Zero;
                        break;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switch (currentRotationPreset)
                {
                    case RotationPreset.Zero:
                        m_targetRotation = Quaternion.Euler(60, 270, 0);
                        
                        wMoveVector = new Vector3(-1f, 0f, 0f);
                        aMoveVector = new Vector3(0, 0f, -1f);
                        sMoveVector = new Vector3(1f, 0f, 0f);
                        dMoveVector = new Vector3(0, 0f, 1f);
                        
                        currentRotationPreset = RotationPreset.TwoHundredSeventy;
                        break;
                    
                    case RotationPreset.Ninty:
                        m_targetRotation = Quaternion.Euler(60, 0, 0);
                        
                        wMoveVector = new Vector3(0f, 0f, 1f);
                        aMoveVector = new Vector3(-1f, 0f, 0f);
                        sMoveVector = new Vector3(0f, 0f, -1f);
                        dMoveVector = new Vector3(1f, 0f, 0f);
                        
                        currentRotationPreset = RotationPreset.Zero;
                        break;
                    
                    case RotationPreset.HundredEighty:
                        m_targetRotation = Quaternion.Euler(60, 90, 0);
                        
                        wMoveVector = new Vector3(1f, 0f, 0f);
                        aMoveVector = new Vector3(0, 0f, 1f);
                        sMoveVector = new Vector3(-1f, 0f, 0f);
                        dMoveVector = new Vector3(0, 0f, -1f);
                        
                        currentRotationPreset = RotationPreset.Ninty;
                        break;
                    
                    case RotationPreset.TwoHundredSeventy:
                        m_targetRotation = Quaternion.Euler(60, 180, 0);
                        
                        wMoveVector = new Vector3(0f, 0f, -1f);
                        aMoveVector = new Vector3(1f, 0f, 0f);
                        sMoveVector = new Vector3(0f, 0f, 1f);
                        dMoveVector = new Vector3(-1f, 0f, 0f);
                        
                        currentRotationPreset = RotationPreset.HundredEighty;
                        break;
                }
            }
            
            transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, Time.deltaTime * rotationSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                if (LookForGameObject(out RaycastHit hit))
                {
                    Grid.Item hitGridItem = hit.collider.gameObject.GetComponentInParent<Grid.Item>();
                    if (hitGridItem != null)
                    {
                        m_cameraTarget = hitGridItem;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                cameraZoom = 10f;
                transform.position = new Vector3(transform.position.x, 8.66f, transform.position.z);
                m_cameraTarget = null;
                
                StatManager.Instance.SelectedStatObject.SetActive(false);
            }
        }

        private bool LookForGameObject(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit);
        }
    }

    public enum RotationPreset
    {
        Zero,
        Ninty,
        HundredEighty,
        TwoHundredSeventy
    }
}