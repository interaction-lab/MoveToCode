using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ResourcePathConstants : MonoBehaviour {

        // Prefabs
        public static string PrefabFolder = "Prefabs/";
        public static string ExerciseJsonFolder = "ExerciseJsons/";
        public static string CodeBlockPrefabFolder = PrefabFolder + "CodeBlockPrefabs/";

        public static string VariableCodeBlockPrefab = CodeBlockPrefabFolder + "VariableCodeBlock";
        public static string CodeBlockTextPrefab = CodeBlockPrefabFolder + "CodeBlockText";
        public static string VariableCodeBlockCollectionPrefab = CodeBlockPrefabFolder + "VariableCodeBlockCollection";
        public static string PrintCodeBlockPrefab = CodeBlockPrefabFolder + "PrintCodeBlock";
        public static string ConditionBlockPrefab = CodeBlockPrefabFolder + "ConditionalCodeBlock";
        public static string IfCodeBlockPrefab = CodeBlockPrefabFolder + "IfCodeBlock";
        public static string IntCodeBlockPrefab = CodeBlockPrefabFolder + "IntCodeBlock";
        public static string MathCodeBlockPrefab = CodeBlockPrefabFolder + "MathCodeBlock";
        public static string SetVariableCodeBlockPrefab = CodeBlockPrefabFolder + "SetVariableCodeBlock";
        public static string StringCodeBlockPrefab = CodeBlockPrefabFolder + "StringCodeBlock";
        public static string WhileCodeBlockPrefab = CodeBlockPrefabFolder + "WhileCodeBlock";
        public static string CharCodeBlockPrefab = CodeBlockPrefabFolder + "CharCodeBlock";
        public static string ArrayCodeBlockPrefab = CodeBlockPrefabFolder + "ArrayCodeBlock";
        public static string ArrayElementPrefab = CodeBlockPrefabFolder + "ArrayElement";
        public static string ArrayIndexCodeBlockPrefab = CodeBlockPrefabFolder + "ArrayIndexCodeBlock";
        public static string ExercisePrefab = PrefabFolder + "Exercise";



        public static Dictionary<string, GameObject> codeBlockDictionary = new Dictionary<string, GameObject> {
            {"Print", Resources.Load<GameObject>(PrintCodeBlockPrefab)},
            {"Conditional", Resources.Load<GameObject>(ConditionBlockPrefab)},
            {"If", Resources.Load<GameObject>(IfCodeBlockPrefab)},
            {"Int", Resources.Load<GameObject>(IntCodeBlockPrefab)},
            {"Math", Resources.Load<GameObject>(MathCodeBlockPrefab)},
            {"SetVar", Resources.Load<GameObject>(SetVariableCodeBlockPrefab)},
            {"String", Resources.Load<GameObject>(StringCodeBlockPrefab)},
            {"While", Resources.Load<GameObject>(WhileCodeBlockPrefab)},
            {"Char", Resources.Load<GameObject>(CharCodeBlockPrefab)},
            {"Array", Resources.Load<GameObject>(ArrayCodeBlockPrefab)},
            {"ArrayIndex", Resources.Load<GameObject>(ArrayIndexCodeBlockPrefab)},
            {"Variable", Resources.Load<GameObject>(VariableCodeBlockPrefab)}
        };

        // Materials
        public static string MaterialFolder = "Materials/";
        public static string OutlineSnapColliderMaterial = MaterialFolder + "OutlineSnapCollider";
        public static string OutlineCodeBlockMaterial = MaterialFolder + "OutlineCodeBlock";
        public static string FakeButtonPressParticleMaterial = MaterialFolder + "FakeButtonPressParticle";
        public static string locLedOnMaterial = MaterialFolder + "ChestLED";
        public static string locLedOffMaterial = MaterialFolder + "ChestLED_off";

        // Audio
        public static string AudioFolder = "Audio/";
        public static string CorrectSound = AudioFolder + "correct";
        public static string IncorrectSound = AudioFolder + "incorrect";
        public static string PoofSound = AudioFolder + "poof";
        public static string PopSound = AudioFolder + "pop";
        public static string SnapSound = AudioFolder + "snap";
        public static string SpawnSound = AudioFolder + "spawn";
        public static string ComputerNoises = AudioFolder + "ComputerNoises";

        public static string SpeechFolder = AudioFolder + "PreloadSpeech/";
        public static string SpeechCacheFolder = AudioFolder + "CacheSpeech/";

        public static string CongratulationPhrases = SpeechFolder + "preloadCongratulation";
        public static string EncouragementPhrases = SpeechFolder + "preloadEncouragement";
        public static string CachePhrases = SpeechCacheFolder + "cachePhrase";

        public static string KuriSoundFolder = AudioFolder + "KuriAudio/";
        public static string KuriPositiveSoundFolder = KuriSoundFolder + "Positive/";
        public static string KuriNegativeSoundFolder = KuriSoundFolder + "Negative/";

        public static string KuriILoveYouSound = KuriPositiveSoundFolder + "i_love_you";
        public static string KuriGreetingSound = KuriPositiveSoundFolder + "Greeting";
        public static string KuriYippeSound = KuriPositiveSoundFolder + "S81_YIPPEEE_2";

        public static string KuriBangDownSound = KuriNegativeSoundFolder + "BANG_GOINGDOWN";
        public static string KuriFart = KuriNegativeSoundFolder + "Fart";
        public static string KuriPonderSad = KuriNegativeSoundFolder + "PONDER_SAD";

        // ARImages
        public static string str_wall_1 = "wall_1";
        public static string str_wall_2 = "wall_2";
        public static string str_wall_3 = "wall_3";
        public static string kuri_start = "kuri_start";
        public static string goal = "goal";
        public static string turn_1 = "turn_1";
        public static string connection = "Connection";

        public static string ARImageFolder = "ARImageReferences/";
        public static string MazePrefabFolder = ARImageFolder + "MazePrefabs/";
        public static string Wall_1 = MazePrefabFolder + str_wall_1;
        public static string Wall_2 = MazePrefabFolder + str_wall_2;
        public static string Wall_3 = MazePrefabFolder + str_wall_3;
        public static string Goal = MazePrefabFolder + goal;
        public static string Turn_1 = MazePrefabFolder + turn_1;

        public static string ConnectionP = MazePrefabFolder + connection;

        public static Dictionary<string, GameObject> mazeObjectDict = new Dictionary<string, GameObject> {
            {str_wall_1, Resources.Load<GameObject>(Wall_1)},
            {str_wall_2, Resources.Load<GameObject>(Wall_2)},
            {str_wall_3, Resources.Load<GameObject>(Wall_3)},
            {goal, Resources.Load<GameObject>(Goal)},
            {turn_1, Resources.Load<GameObject>(Turn_1)},
            {kuri_start, Resources.Load<GameObject>(ARImageFolder + kuri_start)},
        };

        // AWS Credentials
        public static string AWSPollyAPI = "PollyAPI1";
    }
}
