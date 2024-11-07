// Team.cs
public class Team : MonoBehaviour
{
    private Athlete[] players;
    private Team opposingTeam;

    public void Initialize(TeamConfigSO config, GameObject athletePrefab, Transform[] spawnPoints)
    {
        // Handle case where there are more spawn points than configured players
        int playerCount = Mathf.Min(config.playerSkills.Length, spawnPoints.Length);
        players = new Athlete[playerCount];
        
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerObj = Instantiate(athletePrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            Athlete athlete = playerObj.GetComponent<Athlete>();
            athlete.Initialize(config.playerSkills[i]);
            players[i] = athlete;
        }
    }

    public void SetOpposingTeam(Team opponent)
    {
        opposingTeam = opponent;
        foreach (Athlete player in players)
        {
            // opponent might be null in tutorial/debug scenes
            Athlete[] opponentPlayers = opponent?.GetPlayers() ?? new Athlete[0];
            player.SetTeamReferences(players, opponentPlayers);
        }
    }

    public Athlete[] GetPlayers() => players;
}

// Athlete.cs
public class Athlete : MonoBehaviour
{
    private SkillsSO skills;
    private Athlete[] teammates;
    private Athlete[] opponents;

    public void Initialize(SkillsSO athleteSkills)
    {
        skills = athleteSkills;
    }

    public void SetTeamReferences(Athlete[] allTeamPlayers, Athlete[] opposingPlayers)
    {
        // Create array for teammates (excluding self)
        teammates = new Athlete[allTeamPlayers.Length - 1];
        int index = 0;
        
        for (int i = 0; i < allTeamPlayers.Length; i++)
        {
            if (allTeamPlayers[i] != this)
            {
                teammates[index] = allTeamPlayers[i];
                index++;
            }
        }

        opponents = opposingPlayers;
    }

    public void PassBall()
    {
        // Check if we have any teammates before attempting to pass
        if (teammates.Length > 0)
        {
            Athlete nearestTeammate = FindNearestPlayer(teammates);
            // Implement passing logic
        }
        else
        {
            // Handle solo player scenario (maybe bounce the ball or do a serve)
            HandleSoloAction();
        }
    }

    public void SpikeAtTarget()
    {
        // Check if we have any opponents before attempting to spike
        if (opponents.Length > 0)
        {
            Athlete bestTarget = FindBestSpikeTarget(opponents);
            // Implement spiking logic
        }
        else
        {
            // Handle no-opponent scenario (maybe spike to a specific court location)
            HandleSoloSpike();
        }
    }

    private void HandleSoloAction()
    {
        // Implement tutorial/practice behavior
        Debug.Log("Performing solo action");
    }

    private void HandleSoloSpike()
    {
        // Implement spike behavior for when there are no opponents
        Debug.Log("Performing solo spike");
    }
}

// MatchManager.cs
public class MatchManager : MonoBehaviour
{
    [SerializeField] private MatchConfigSO matchConfig;
    [SerializeField] private Transform[] teamASpawnPoints;
    [SerializeField] private Transform[] teamBSpawnPoints;
    [SerializeField] private bool isTutorialMode;

    private Team teamA;
    private Team teamB;

    private void Start()
    {
        InitializeMatch();
    }

    private void InitializeMatch()
    {
        // Create Team A
        GameObject teamAObj = new GameObject("Team A");
        teamA = teamAObj.AddComponent<Team>();
        teamA.Initialize(matchConfig.teamA, matchConfig.athletePrefab, teamASpawnPoints);

        if (!isTutorialMode)
        {
            // Only create Team B if not in tutorial mode
            GameObject teamBObj = new GameObject("Team B");
            teamB = teamBObj.AddComponent<Team>();
            teamB.Initialize(matchConfig.teamB, matchConfig.athletePrefab, teamBSpawnPoints);
        }

        // Set up team references (teamB might be null in tutorial mode)
        teamA.SetOpposingTeam(teamB);
        if (teamB != null)
        {
            teamB.SetOpposingTeam(teamA);
        }
    }
}
