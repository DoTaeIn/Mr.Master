using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoSO
{
    private static string drinkCSVPath = "/CSV/Drink.csv"; // CSV 파일 경로
    public static string outputFolder = "Assets/Scripts/Bar/Sos"; // ScriptableObject 저장 폴더

    [MenuItem("Utilities/Generate Drinks")]
    public static void StartParsing()
    {
        string[] splitData = File.ReadAllLines(Application.dataPath + drinkCSVPath);

        if (splitData == null || splitData.Length <= 1)
        {
            Debug.LogError("CSV 파일이 비어있거나 유효하지 않습니다!");
            return;
        }

        // 출력 폴더 확인 및 생성
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        for (int i = 1; i < splitData.Length; i++) // 첫 번째 행은 헤더로 간주
        {
            string[] columns = splitData[i].Split(',');

            if (columns.Length < 7)
            {
                Debug.LogError($"잘못된 행 형식: {splitData[i]}");
                continue;
            }

            DrinkSO drinkData = ScriptableObject.CreateInstance<DrinkSO>();

            // ID 파싱
            if (!int.TryParse(columns[1], out drinkData.id))
            {
                Debug.LogError($"ID가 잘못되었습니다 (행 {i}): {columns[1]}");
                continue;
            }

            // 이름
            drinkData.name = columns[0];

            // 가격 파싱
            if (!float.TryParse(columns[2], out drinkData.price))
            {
                Debug.LogError($"가격이 잘못되었습니다 (행 {i}): {columns[2]}");
                continue;
            }

            // 알코올 도수 파싱
            if (!float.TryParse(columns[3], out drinkData.proof))
            {
                Debug.LogError($"알코올 도수가 잘못되었습니다 (행 {i}): {columns[3]}");
                continue;
            }

            // 양 파싱
            if (!float.TryParse(columns[4], out drinkData.amount))
            {
                Debug.LogError($"양이 잘못되었습니다 (행 {i}): {columns[4]}");
                continue;
            }

            // 색상 파싱 - "?" 처리
            string rgbStr = columns[5].Replace("\"", "").Replace("(", "").Replace(")", "").Trim();
            string[] rgb = rgbStr.Split('?');
            if (rgb.Length == 4 &&
                float.TryParse(rgb[0], out float r) &&
                float.TryParse(rgb[1], out float g) &&
                float.TryParse(rgb[2], out float b) &&
                float.TryParse(rgb[3], out float a))
            {
                drinkData.color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
            else
            {
                Debug.LogError($"색상 정보가 잘못되었습니다 (행 {i}): {columns[5]}");
                continue;
            }

            // 맛 파싱 - "?" 처리
            string[] tastes = columns[6].Split('?');
            drinkData.tastes = new List<string>();
            foreach (string taste in tastes)
            {
                drinkData.tastes.Add(taste.Trim());
            }

            // ScriptableObject 저장
            string assetPath = $"{outputFolder}/{drinkData.name}.asset";
            AssetDatabase.CreateAsset(drinkData, assetPath);
            AssetDatabase.SaveAssets();
        }

        Debug.Log("ScriptableObjects 생성이 완료되었습니다!");
    }
}
