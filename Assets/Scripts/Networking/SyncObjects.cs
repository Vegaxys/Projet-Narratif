using UnityEngine;
using Photon.Pun;

public class SyncObjects : MonoBehaviourPun, IPunObservable{

    private Vector3 objPos;
    private Vector3 objScale;
    private Quaternion objRot;
    [SerializeField]private float lerpSpeed = 8;
    private void Update() {
        if (!photonView.IsMine) {
            UpdateTransform();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(gameObject.transform.position);
            stream.SendNext(gameObject.transform.localScale);
            stream.SendNext(gameObject.transform.rotation);
            
        } else if (stream.IsReading) {
            objPos = (Vector3)stream.ReceiveNext();
            objScale = (Vector3)stream.ReceiveNext();
            objRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private void UpdateTransform() {
        gameObject.transform.position = Vector3.Lerp(transform.position, objPos, lerpSpeed * Time.deltaTime);
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, objScale, lerpSpeed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, objRot, lerpSpeed * Time.deltaTime);
    }
}
