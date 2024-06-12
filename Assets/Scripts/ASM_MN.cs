using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();

    private void Start()
    {
        createRegion();
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        // Tính toán xếp hạng dựa trên điểm số
        if (score < 200)
            return "Đồng";
        else if (score < 600)
            return "Bạc";
        else if (score < 1050)
            return "Vàng";
        else
            return "Kim cương";
    }

    public void YC1()
    {
        // Lấy các giá trị từ ScoreKeeper
        string name = ScoreKeeper.Instance.GetUserName();
        int id = ScoreKeeper.Instance.GetID();
        int idR = ScoreKeeper.Instance.GetIDregion();
        int score = ScoreKeeper.Instance.GetScore();

        // Lấy tên vùng dựa theo id
        string regionName = listRegion.FirstOrDefault(r => r.ID == idR)?.Name ?? "Unknown";

        // Thêm thông tin người chơi mới khi nhập từ text
        Region playerRegion1 = new Region(idR, regionName);
        Players player3 = new Players(id, name, score, playerRegion1);
        listPlayer.Add(player3);

        // Thêm người chơi giả lập để kiểm tra
        Players player1 = new Players(id, "Tuần", 500, new Region(2, "VN2"));
        listPlayer.Add(player1);
        Players player2 = new Players(id, "Hùng", 100000, new Region(3, "JS"));
        listPlayer.Add(player2);
    }
    public void YC2()
    {
        // Duyệt và in các thông tin của từng đối tượng trong Players ra
        foreach (Players player in listPlayer)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Player Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + "-Rank:" + rank);

        }
    }
    public void YC3()
    {
        if (listPlayer.Count == 0)
        {
            Debug.Log("không có người chơi nào khác.");
            return;
        }

        int currentPlayerScore = listPlayer[0].Score;
        var less = listPlayer.Where(Pr => Pr.Score < currentPlayerScore);

        if (!less.Any())
        {
            Debug.Log("không có người chơi nào khác tệ hơn bạn.");
            return;
        }

        Debug.Log("Player có score bé hơn score hiện tại của người chơi.");
        foreach (var player in less)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - score: " + player.Score + " - Rank: " + rank);
        }

    }
    public void YC4()
    {
        // sinh viên viết tiếp code ở đây
        int currentPlayerId = ScoreKeeper.Instance.GetID();
        var player = listPlayer.FirstOrDefault(p => p.Id == currentPlayerId);
        if (player != null)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Current Player: Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
        else
        {
            Debug.Log("Player not found.");
        }

    }
    public void YC5()
    {
        // sinh viên viết tiếp code ở đây

        var sortedPlayers = listPlayer.OrderByDescending(p => p.Score).ToList();
        Debug.Log("Danh sách người chơi theo thứ tự score giảm dần:");
        foreach (var player in sortedPlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }

    }
    public void YC6()
    {
        // sinh viên viết tiếp code ở đây

        var sortedPlayers = listPlayer.OrderBy(p => p.Score).Take(5).ToList();
        Debug.Log("Top 5 người chơi có score thấp nhất:");
        foreach (var player in sortedPlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
    }
    public void YC7()
    {
        // sinh viên viết tiếp code ở đây

        Thread bxhThread = new Thread(CalculateAndSaveAverageScoreByRegion);
        bxhThread.Name = "BXH";
        bxhThread.Start();

    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        // sinh viên viết tiếp code ở đây

        var regionGroups = listPlayer.GroupBy(p => p.PlayerRegion);
        using (StreamWriter writer = new StreamWriter("bxhRegion.txt"))
        {
            foreach (var group in regionGroups)
            {
                string regionName = group.Key.Name;
                double averageScore = group.Average(p => p.Score);
                writer.WriteLine("Region: " + regionName + " - Average Score: " + averageScore);
            }
        }
        Debug.Log("Đã lưu thông tin vào bxhRegion.txt");
    }

}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;
    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public Region PlayerRegion { get; set; }

    // Constructor
    public Players(int id, string name, int score, Region region)
    {
        Id = id;
        Name = name;
        Score = score;
        PlayerRegion = region;
    }
}