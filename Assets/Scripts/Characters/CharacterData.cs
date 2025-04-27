using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character")]
public class CharacterData : ScriptableObject
{
    public Sprite portrait;
    public GameTimestamp birthday;
    public List<ItemData> likes;
    public List<ItemData> dislikes;
    public GameObject prefab;

    [Header("Dialogue")]
    public List<DialogueLine> onFirstMeet;
    public List<DialogueLine> defaultDialogue;

    public List<DialogueLine> likedGiftDialogue;
    public List<DialogueLine> dislikedGiftDialogue;
    public List<DialogueLine> neutralGiftDialogue;
    
    public List<DialogueLine> birthdayLikedGiftDialogue;
    public List<DialogueLine> birthdayDislikedGiftDialogue;
    public List<DialogueLine> birthdayNeutralGiftDialogue;
}