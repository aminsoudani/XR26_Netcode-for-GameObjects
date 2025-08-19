using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;  // Add this for UnityAction
using System.Collections;  // Added for IEnumerator support
public class PlayerChat : NetworkBehaviour
{
    [SerializeField] Player player;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        StartCoroutine(OnStart());  // Fixed typo from StartCourotine
    }
    private IEnumerator OnStart()
    {
        yield return new WaitForSeconds(2); // Wait for the UI to be ready
        ChatUI.Instance.OnMessageSubmit.AddListener(() => SendChatMessage());  // Fixed the delegate
        yield return null;
    }
    public void SendChatMessage()
    {
        SendMessageServerRpc(ChatUI.Instance.chatInput.text);
    }
    [ServerRpc]
    public void SendMessageServerRpc(string text)
    {
        // Display the message in the chat UI
        ReceiveMessageClientRpc(player.playerName.Value.ToString(), text);
    }
    private IEnumerator HandleChatMessage()  // IEnumerator will now be recognized
    {
        yield return null;
    }
    [ClientRpc]  // Fixed casing from clientRpc to ClientRpc
    public void ReceiveMessageClientRpc(string name, string text)
    {
        // Display the message in the chat UI
        ChatUI.Instance.CreateChatMessage(name, text);
    }
}