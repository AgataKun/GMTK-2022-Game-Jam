using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransformNoiseMovement : MonoBehaviour
{
    [SerializeField]
    private Transform transformToNoiseMove;
    [SerializeField]
    private bool isActive = true;
    public NoiseShake3D movementNoiseShake;
    public NoiseShake3D rotationNoiseShake;

    public bool IsActive { get => isActive; private set => isActive = value; }

    private void Awake()
    {
        movementNoiseShake.Init();
        rotationNoiseShake.Init();
    }

    private void Update()
    {
        if(!IsActive)
            return;

        movementNoiseShake.ApplyCalculatedNoiseOffset(transformToNoiseMove.Translate);
        rotationNoiseShake.ApplyCalculatedNoiseOffset(transformToNoiseMove.Rotate);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    [Serializable]
    public class NoiseShake3D
    {
        public float amplitudeGain = 1f;
        public float frequencyGain = 1f;

        private Vector3 axisSeeds = Vector3.zero;
        private float elapsedTime = 0f;
        private Vector3 previousNoiseOffset = Vector3.zero;

        public void Init()
        {
            axisSeeds = Random.insideUnitSphere;
        }

        public void ApplyCalculatedNoiseOffset(Action<Vector3> applyNoiseAction)
        {
            elapsedTime += Time.deltaTime * frequencyGain;
            Vector3 currentNoiseOffset = MathUtils.RandomSmoothOffsetNoise3D(elapsedTime, amplitudeGain, 1f, axisSeeds.x, axisSeeds.y, axisSeeds.z);
            applyNoiseAction(currentNoiseOffset - previousNoiseOffset);
            previousNoiseOffset = currentNoiseOffset;
        }
    }
}
