﻿using BulletInBoardServer.Services.Surveys.DelayedOperations;

namespace BulletInBoardServer.Test.Services.Surveys.DelayedOperations;

public class AutomaticSurveyOperationsDispatcherMock : IAutomaticSurveyOperationsDispatcher
{
    public int ConfigureAutoClosingCalled { get; private set; }
    public int DisableAutoClosingCalled { get; private set; }
    
    
    
    public void ConfigureAutoClosing(Guid surveyId, DateTime closeAt) => 
        ConfigureAutoClosingCalled++;

    public void DisableAutoClosing(Guid surveyId) => 
        DisableAutoClosingCalled++;
}