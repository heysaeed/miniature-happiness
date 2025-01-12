using Overlay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Employee
{
    [RequireComponent(typeof(EmployeeImpl))]
    [AddComponentMenu("Scripts/Employee/Employee.View")]
    public partial class View : MonoBehaviour
    {
        private EmployeeImpl employee;

        [Required]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float animationSpeedMultiplier = 1.5f;

        [Required]
        [SerializeField]
        private NavMeshAgent agent;

        private void Start()
        {
            employee = GetComponent<EmployeeImpl>();
        }

        private void Update()
        {
            UpdateStressOverlay();
            animator.SetFloat(
                "WalkSpeed",
                agent.velocity.magnitude / agent.speed * animationSpeedMultiplier
            );
        }

        public void RevertOverlays()
        {
            RevertStressOverlay();
            RevertExtendedInfoOverlay();
        }
    }

    [RequireComponent(typeof(PostEffectMaskRenderer))]
    public partial class View : IOverlayRenderer<Stress>
    {
        [Required]
        [SerializeField]
        private SkinnedMeshRenderer meshRenderer;

        [Required]
        [SerializeField]
        private Material baseMaterial;

        [Required]
        [SerializeField]
        private Material stressMaterial;

        private Stress appliedStressOverlay;

        private PostEffectMaskRenderer maskRenderer;

        public void ApplyOverlay(Stress overlay)
        {
            if (maskRenderer == null)
            {
                maskRenderer = GetComponent<PostEffectMaskRenderer>();
                PostEffectMask mask = Camera.main.GetComponent<PostEffectMask>();
                if (mask == null)
                {
                    Debug.LogError("Unable to find PostEffectMask on main camera");
                }
                maskRenderer.mask = mask;
            }

            appliedStressOverlay = overlay;
            meshRenderer.material = stressMaterial;
        }

        private void UpdateStressOverlay()
        {
            if (appliedStressOverlay == null)
            {
                return;
            }

            meshRenderer.material.color = appliedStressOverlay.GetCurrentColor(employee.Stress);
        }

        public void RevertStressOverlay()
        {
            appliedStressOverlay = null;
            meshRenderer.material = baseMaterial;
        }
    }

    public partial class View : IOverlayRenderer<ExtendedEmployeeInfo>
    {
        [SerializeField]
        [Required]
        [ChildGameObjectsOnly]
        private ExtendedInfo.View extendedInfoView;

        public void ApplyOverlay(ExtendedEmployeeInfo overlay)
        {
            extendedInfoView.gameObject.SetActive(true);
        }

        public void RevertExtendedInfoOverlay()
        {
            extendedInfoView.gameObject.SetActive(false);
        }
    }
}
