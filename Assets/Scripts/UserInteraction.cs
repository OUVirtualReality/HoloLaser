using UnityEngine;
using UnityEngine.XR.WSA.Input;

/// <summary>
/// Custom Action Manager class for OU's VR Club
/// </summary>
public class UserInteraction : MonoBehaviour
{

    #region Private Fields
    private InteractionSourceState _interactionSourceState;
    private InteractionSourcePressedEventArgs _interactionSourcePressedEventArgs;
    private InteractionSourceReleasedEventArgs _interactionSourceReleasedEventArgs;
    #endregion Private Fields


    #region Unity Start/Destroy stuff
    void Start()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_SourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_SourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_SourcePressed;
        InteractionManager.InteractionSourceReleased += InteractionManager_SourceReleased;
    }

    void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= InteractionManager_SourceDetected;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_SourceUpdated;
        InteractionManager.InteractionSourceLost -= InteractionManager_SourceLost;
        InteractionManager.InteractionSourcePressed -= InteractionManager_SourcePressed;
        InteractionManager.InteractionSourceReleased -= InteractionManager_SourceReleased;
    }
    #endregion Unity Start/Destroy stuff


    #region Public Properties
    /// <summary>
    /// Information about the current state of user interaction.
    /// </summary>
    public InteractionSourceState State
    {
        get { return _interactionSourceState; }
        private set { _interactionSourceState = value; }
    }

    /// <summary>
    /// Information about the last "press" interaction
    /// </summary>
    public InteractionSourcePressedEventArgs InteractionSourcePressedEventArgs
    {
        get { return _interactionSourcePressedEventArgs; }
        private set { _interactionSourcePressedEventArgs = value; }
    }

    /// <summary>
    /// Information about the last "release" interaction.
    /// </summary>
    public InteractionSourceReleasedEventArgs InteractionSourceReleasedEventArgs
    {
        get { return _interactionSourceReleasedEventArgs; }
        private set { _interactionSourceReleasedEventArgs = value; }
    }

    /// <summary>
    /// Head pose information (orientation and position).
    /// </summary>
    public Pose HeadPose
    {
        get { return State.headPose; }
    }
    #endregion Public Properties


    #region Class Methods
    void InteractionManager_SourceDetected(InteractionSourceDetectedEventArgs args)
    {
        // Source was detected
        // state has the current state of the source including id, position, kind, etc.
        State = args.state;
    }

    void InteractionManager_SourceLost(InteractionSourceLostEventArgs args)
    {
        // Source was lost. This will be after a SourceDetected event and no other events for this source id will occur until it is Detected again
        // state has the current state of the source including id, position, kind, etc.
        State = args.state;
    }

    void InteractionManager_SourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        // Source was updated. The source would have been detected before this point
        // state has the current state of the source including id, position, kind, etc.
        State = args.state;
    }

    void InteractionManager_SourcePressed(InteractionSourcePressedEventArgs args)
    {
        // Source was pressed. This will be after the source was detected and before it is released or lost
        // state has the current state of the source including id, position, kind, etc.
        State = args.state;
        InteractionSourcePressedEventArgs = args;

    }

    void InteractionManager_SourceReleased(InteractionSourceReleasedEventArgs args)
    {
        // Source was released. The source would have been detected and pressed before this point. This event will not fire if the source is lost
        // state has the current state of the source including id, position, kind, etc.
        State = args.state;
        InteractionSourceReleasedEventArgs = args;
    }

    #endregion Class Methods


}
