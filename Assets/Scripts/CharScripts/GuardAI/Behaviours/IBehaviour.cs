using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

/*
 * Unique behaviours for each guard
 * guard reacts differently to player tools depending on these behaviours
 */
public interface IBehaviour
{

    string ReturnBehaviourDescription();

}
