using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public string[] enemyPoolTags; // 적 오브젝트 풀의 태그 배열
    public float spawnInterval = 3f; // 적 생성 간격
    public float spawnRadius = 10f; // 스폰 반경

    private float timer = 0f;

    private void Update()
    {
        // 일정 시간마다 적 생성
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        // 무작위 스폰 위치 및 적 오브젝트 풀 태그 선택
        Vector3 spawnPosition = GetRandomSpawnPosition();
        string randomTag = enemyPoolTags[Random.Range(0, enemyPoolTags.Length)];

        // NavMesh에서 유효한 스폰 위치인지 확인
        if (!IsValidSpawnPosition(spawnPosition))
        {
            Debug.LogWarning("Invalid spawn position: " + spawnPosition);
            return;
        }

        // 오브젝트 풀에서 적 오브젝트 가져오기
        GameObject enemy = GameManager.Instance.pool.SpawnFromPool(randomTag);

        if (enemy != null)
        {
            // 적의 위치 및 회전 설정
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            // 적 초기화 및 기타 설정
            InitializeEnemy(enemy);
        }
        else
        {
            Debug.LogWarning("Failed to spawn enemy from the pool. Tag: " + randomTag);
        }
    }

    // NavMesh에서 유효한 위치인지 확인하는 메서드
    private bool IsValidSpawnPosition(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, spawnRadius, NavMesh.AllAreas);
    }

    // 적 오브젝트의 초기화 및 설정을 수행하는 메서드
    private void InitializeEnemy(GameObject enemy)
    {
        // 여기에 필요한 초기화 및 설정 코드를 추가하세요.
    }

    // 무작위 스폰 위치를 반환하는 메서드
    private Vector3 GetRandomSpawnPosition()
    {
        // 스폰 위치를 무작위로 결정
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;

        // 유효한 NavMesh 위치인지 확인하여 반환
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // 실패 시 현재 위치 반환
        return transform.position;
    }
}
