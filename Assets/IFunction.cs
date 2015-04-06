using UnityEngine;
using System.Collections;

public interface IFunction<A, B, C>
{
    C Apply(A a, B c);
}
