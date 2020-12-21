using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CAnimationMoveComponent : CMoveComponent
{
    [HideInInspector]
    public Animator animator;
    private GameObject animObject;
    [NonSerialized]
    protected bool useAnimation = true;
    protected bool bAborted=false;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animObject = transform.GetChild(0).gameObject;
    }
    protected override void Update()
    {
        if(!useAnimation)
            base.Update();
    }
    public override bool RequestMove(Vector2Int[] pathArr)
    {
        bAborted = false;
        bool res = base.RequestMove(pathArr);
        if (res)
        {
            animator.Play("Move");
            Vector3 dir = curTargetPosition - transform.position;
            if (dir.magnitude > 0.01)
                (chess as GChess).FaceToward(dir);
        }
        return res;
    }
    protected void DisableAnimationOnce()
    {
        useAnimation = false;
        eFinishPath.AddListenerForOnce(() => useAnimation = true);
    }
    public override bool RequestDirectMove(Vector2Int destination)
    {
        bAborted = false;
        DisableAnimationOnce();
        return base.RequestMove(new Vector2Int[] {destination});
    }
    public override bool RequestJumpMove(Vector2Int destination)
    {
        bAborted = false;
        DisableAnimationOnce();
        return base.RequestJumpMove(destination);
    }
    public override void AbortMove()
    {
        bAborted = true;
        animator.Play("Idle");
        base.AbortMove();
        
    }
    public void OneMoveComplete()
    {
        if(bAborted)
        {
            return;
        }
        transform.position = curTargetPosition;
        if(!NextPosition())
        {
            animator.Play("Idle");
        }
        else
        {
            Vector3 dir = curTargetPosition - transform.position;
            transform.rotation=Quaternion.LookRotation(dir.normalized,Vector3.up);
            animator.Play("Move");
        }
    }
}
