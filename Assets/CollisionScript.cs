using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionScript : MonoBehaviour
{

    public GameObject player;
    public EnemySpawner spawner;
    public Vector3 scaleChange;
    public float seconds_before_reset;
    public Score score;
    public int scores = 0;
    private Player_Movement movement;

    void Start()
    {
        movement = GetComponent<Player_Movement>();
    }

    public Animator animator;
    //public Animation eating;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Enemy")
            return;

        Vector3 enemy = collision.gameObject.transform.localScale;
        if (enemy.x <= player.transform.localScale.x && enemy.y <= player.transform.localScale.y) {
            Destroy(collision.gameObject);
            spawner.enemyCount -= 1;
            player.transform.localScale += scaleChange;
            scores += 1;
            score.scoreText.text = scores.ToString();
            float x = animator.GetFloat("Horizontal");
            if (x < 0)
                animator.Play("Base Layer.Player_Eat_Left");
            else if (x > 0)
                animator.Play("Base Layer.Player_Eat_Right");
        } else {
            StartCoroutine(Coroutine());
            scores = 0;
        }
    }

    IEnumerator Coroutine() {
        movement.moveSpeed = 0f;
        animator.Play("Base Layer.Player_Death");
        spawner.enemyCount = 30;
        yield return new WaitForSeconds(seconds_before_reset);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
            Destroy(enemy);
        player.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        player.transform.position = new Vector3(0f, 0f, -1);
        spawner.enemyCount = 0;
        score.scoreText.text = "0";
        movement.moveSpeed = 5f;
    }
}
