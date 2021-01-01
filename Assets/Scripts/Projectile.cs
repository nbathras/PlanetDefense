using UnityEngine;

public class Projectile : MonoBehaviour {
    public static Projectile Create(Vector3 spawnPosition, Vector3 targetPosition) {
        Transform pfProjectile = Resources.Load<Transform>("pfProjectile");
        Transform projectileTransform = Instantiate(pfProjectile, spawnPosition, Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        Vector3 normalizedDirection = targetPosition - spawnPosition;

        projectile.normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0f).normalized;
        projectile.targetPostion = new Vector3(targetPosition.x, targetPosition.y, 0f);

        return projectile;
    }

    private enum ProjectileState { PreExplode, Flash, Explode, Fade };

    /* Float Timers */
    [Header("Timers")]
    [SerializeField] private float flashTimerMax;
    private float flashTimer;
    [SerializeField] private float explodeTimerMax;
    private float explodeTimer;
    [SerializeField] private float fadeTimerMax;
    private float fadeTimer;

    /* Transform Holders */
    [Header("Transform Holders")]
    [SerializeField] private Transform projectileHolder;
    [SerializeField] private Transform flashHolder;
    [SerializeField] private Transform explosionHolder;

    /* Other Parameters */
    [Header("Other Parameters")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float maxExplosionSize;

    /* Cache */
    private SpriteRenderer[] explosionSprites;

    private Vector3 targetPostion;
    private Vector3 normalizedDirection;

    private ProjectileState projectileState;

    private void Awake() {
        flashTimer = flashTimerMax;
        explodeTimer = explodeTimerMax;
        fadeTimer = fadeTimerMax;


        projectileHolder.gameObject.SetActive(true);
        flashHolder.gameObject.SetActive(false);
        explosionHolder.gameObject.SetActive(false);

        explosionSprites = explosionHolder.GetComponentsInChildren<SpriteRenderer>();

        projectileState = ProjectileState.PreExplode;
    }

    private void Update() {
        if (projectileState == ProjectileState.PreExplode) {
            PreExplosionAction();
            if (PreExplosionCondition()) {
                PreExplosionStateChange();
            }
        } else if (projectileState == ProjectileState.Flash) {
            FlashAction();
            if (FlashCondition()) {
                FlashStateChange();
            }
        } else if (projectileState == ProjectileState.Explode) {
            ExplodeAction();
            if (ExplodeCondition()) {
                ExplodeStateChange();
            }
        } else if (projectileState == ProjectileState.Fade) {
            FadeAction();
            if (FadeCondition()) {
                FadeStateChange();
            }
        } else {
            throw new System.Exception("Error: The current state is not handled by the projectile");
        }
    }

    private void Destroy() {
        CannonController.Instance.DestroyProjectile(this);
    }

    /* PreExplode State */
    /*******************/
    private void PreExplosionAction() {
        // Moves projectile to taget at constant speed;
        transform.position += normalizedDirection * Time.deltaTime * projectileSpeed;
    }

    private bool PreExplosionCondition() {
        // Return true when close to target
        return Vector3.Distance(transform.position, targetPostion) < 0.1f;
    }

    private void PreExplosionStateChange() {
        // Hide Projectile
        projectileHolder.gameObject.SetActive(false);
        // Show Flash
        flashHolder.gameObject.SetActive(true);

        // Projectile State: PreExplosion => Flash
        projectileState = ProjectileState.Flash;
    }

    /* Flash State */
    /***************/
    private void FlashAction() {
        // Reduce Flash Timer;
        flashTimer -= Time.deltaTime;
    }

    private bool FlashCondition() {
        return flashTimer < 0;
    }

    private void FlashStateChange() {
        // Hide Flash
        flashHolder.gameObject.SetActive(false);
        // Show Explosion
        explosionHolder.gameObject.SetActive(true);

        // Projectile State: Flash => Explode
        projectileState = ProjectileState.Explode;
    }

    /* Explode State */
    /*****************/
    private void ExplodeAction() {
        explodeTimer -= Time.deltaTime;

        float timerPercent = explodeTimer / explodeTimerMax;

        float explosionSize = (1 - timerPercent) * maxExplosionSize;
        transform.localScale = new Vector3(explosionSize, explosionSize, 0);
    }

    private bool ExplodeCondition() {
        return explodeTimer < 0f;
    }

    private void ExplodeStateChange() {
        projectileState = ProjectileState.Fade;
    }

    /* Fade State */
    /**************/
    private void FadeAction() {
        fadeTimer -= Time.deltaTime;

        float timerPercent = fadeTimer / fadeTimerMax;

        float explosionSize = (.75f * timerPercent + .25f) * maxExplosionSize;
        transform.localScale = new Vector3(explosionSize, explosionSize, 0);

        for (int i = 0; i < explosionSprites.Length; i++) {
            Color c = explosionSprites[i].color;
            explosionSprites[i].color = new Color(c.r, c.g, c.b, timerPercent);
        }
    }

    private bool FadeCondition() {
        return fadeTimer < 0f;
    }

    private void FadeStateChange() {
        Destroy();
    }
}
