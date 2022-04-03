using _NBGames.Scripts.Managers;
using UnityEditor;

namespace _NBGames.Scripts.Editor
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SoundManager _soundManager = (SoundManager) target;
            DrawDefaultInspector();

            if (!_soundManager.PlayerJump)
            {
                EditorGUILayout.HelpBox("Player Jump is not set!", MessageType.Warning);
            }
            
            if (!_soundManager.CoinSound)
            {
                EditorGUILayout.HelpBox("Coin Sound is not set!", MessageType.Warning);
            }

            if (!_soundManager.HealthSound)
            {
                EditorGUILayout.HelpBox("Health Sound is not set!", MessageType.Warning);
            }

            if (!_soundManager.EnemyDeathSound)
            {
                EditorGUILayout.HelpBox("Enemy Death Sound is not set!", MessageType.Warning);
            }

            if (!_soundManager.PlayerHurt)
            {
                EditorGUILayout.HelpBox("Player Hurt is not set!", MessageType.Warning);
            }
            
            if (!_soundManager.PlayerDeathSound)
            {
                EditorGUILayout.HelpBox("Player Death Sound is not set!", MessageType.Warning);
            }

            if (!_soundManager.LevelWinStinger)
            {
                EditorGUILayout.HelpBox("Level Win Stinger is not set!", MessageType.Warning);
            }

            if (!_soundManager.TreasureChestSound)
            {
                EditorGUILayout.HelpBox("Treasure Chest Sound is not set!", MessageType.Warning);
            }

            if (!_soundManager.ItemWooshSound)
            {
                EditorGUILayout.HelpBox("Item Woosh Sound is not set!", MessageType.Warning);
            }
            
            if (!_soundManager.SpringSound)
            {
                EditorGUILayout.HelpBox("Spring Sound is not set!", MessageType.Warning);
            }
        }
    }
}
