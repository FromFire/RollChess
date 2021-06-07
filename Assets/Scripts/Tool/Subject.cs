using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 更新订阅 </para>
///   <para> Model有修改时，通过此类通知已注册的所有观察者 </para>
/// </summary>
public class Subject {

    /// <summary>
    ///   <para> 观察者，即发生修改时调用的函数 </para>
    /// </summary>
    public delegate void Observer();

    /// <summary>
    ///   <para> 观察者列表 </para>
    /// </summary>
    private Dictionary<ModelModifyEvent, List<Observer>> observers;

    public Subject() {
        observers = new Dictionary<ModelModifyEvent, List<Observer>>();
    }

    /// <summary>
    ///   <para> 添加观察者 </para>
    /// </summary>
    public void Attach(ModelModifyEvent modelModifyEvent, Observer observer) {
        // 合法性检查：事件不存在、数组为null
        // 为该事件添加新数组
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null) {
            observers[modelModifyEvent] = new List<Observer>();
        }

        // 添加
        observers[modelModifyEvent].Add(observer);
    }

    /// <summary>
    ///   <para> 移除观察者 </para>
    /// </summary>
    public void Detach(ModelModifyEvent modelModifyEvent, Observer observer) {
        // 合法性检查：事件不存在、数组为null
        // List.Remove在对象不存在时返回false
        // 直接返回
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null) {
            return;
        }

        // 移除
        observers[modelModifyEvent].Remove(observer);
    }

    /// <summary>
    ///   <para> 推送某一事件的修改 </para>
    /// </summary>
    public void Notify(ModelModifyEvent modelModifyEvent) {
        // 合法性检查：事件不存在、数组为null
        // 直接返回
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null)
            return;

        // 推送
        for (int i = 0; i < observers[modelModifyEvent].Count; i++)
            observers[modelModifyEvent][i]();
    }
}

/// <summary>
///   <para> 更新订阅 </para>
///   <para> Model有修改时，通过此类通知已注册的所有观察者 </para>
///   <para> 同时通知被修改的坐标 </para>
/// </summary>
public class PositionSubject {

    /// <summary>
    ///   <para> 观察者，即发生修改时调用的函数 </para>
    /// </summary>
    public delegate void PositionObserver(Vector2Int position);

    /// <summary>
    ///   <para> 观察者列表 </para>
    /// </summary>
    private Dictionary<ModelModifyEvent, List<PositionObserver>> observers = new Dictionary<ModelModifyEvent, List<PositionObserver>>();

    /// <summary>
    ///   <para> 添加观察者 </para>
    /// </summary>
    public void Attach(ModelModifyEvent modelModifyEvent, PositionObserver observer) {
        // 合法性检查：事件不存在、数组为null
        // 为该事件添加新数组
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null) {
            observers[modelModifyEvent] = new List<PositionObserver>();
        }

        // 添加
        observers[modelModifyEvent].Add(observer);
    }

    /// <summary>
    ///   <para> 移除观察者 </para>
    /// </summary>
    public void Detach(ModelModifyEvent modelModifyEvent, PositionObserver observer) {
        // 合法性检查：事件不存在、数组为null
        // List.Remove在对象不存在时返回false
        // 直接返回
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null) {
            return;
        }

        // 移除
        observers[modelModifyEvent].Remove(observer);
    }

    /// <summary>
    ///   <para> 推送某一事件的修改 </para>
    /// </summary>
    public void Notify(ModelModifyEvent modelModifyEvent, Vector2Int position) {
        // 合法性检查：事件不存在、数组为null
        // 直接返回
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null)
            return;

        // 推送
        foreach(PositionObserver observer in observers[modelModifyEvent]) {
            observer(position);
        }
    }

    /// <summary>
    ///   <para> 推送某一事件的修改，涉及一系列坐标 </para>
    /// </summary>
    public void Notify(ModelModifyEvent modelModifyEvent, List<Vector2Int> position) {
        // 合法性检查：事件不存在、数组为null
        // 直接返回
        if(!observers.ContainsKey(modelModifyEvent) || observers[modelModifyEvent] == null)
            return;

        // 推送
        foreach(PositionObserver observer in observers[modelModifyEvent]) {
            foreach(Vector2Int pos in position) {
                observer(pos);
            }
        }
    }
}