function getSelectedQuestionType(element) {
    let selectedQuestionType = element.value;
    let count = element.id.split("-")[2];
    $.ajax({
        type: "GET",
        url: "/survey/get-survey-question-option?questionType=" + selectedQuestionType + "&count=" + count + "&renderFirstTime=true",
        success: function (result) {
            $($(element).parent().siblings()[3]).html(result);
        },
    });
}

function createSurveyQuestion() {
    let fieldsets = document.querySelectorAll("fieldset");
    let maxSurveyCountId = fieldsets.length + 1;
    for (let i = 0; i < fieldsets.length; i++) {
        let fieldsetId = parseInt(fieldsets[i].id);
        if (fieldsetId >= maxSurveyCountId) {
            maxSurveyCountId = fieldsetId + 1;
        }
    }
    $.ajax({
        type: "GET",
        url: "/survey/get-survey-question?count=" + maxSurveyCountId,
        success: function (result) {
            $("#survey-questions-field").append(result);
        },
    });
}

function deleteSurveyQuestion(id) {
    let fieldsets = document.querySelectorAll("fieldset");
    if (fieldsets.length > 1) {
        document.getElementById(id).remove();
    }
}

function createSurveyQuestionOption(id) {
    $.ajax({
        type: "GET",
        url: "/survey/get-survey-question-option?questionType=" + 3 + "&count=" + id + "&renderFirstTime=false",
        success: function (result) {
            $("#question-options-" + id).append(result);
        },
    });
}

function deleteSurveyQuestionOption(element) {
    let elementToRemove = element.parentElement
    let otherElementsCount = document.getElementsByClassName(elementToRemove.classList[0]).length
    if (otherElementsCount > 2) {
        elementToRemove.remove();
    }
}

function prepareSurveyFormData() {
    let checkInputsResult = checkInputs();
    if (checkInputsResult){
        alert("There are empty inputs!");
        return;
    }
    let surveyQuestions = document.querySelectorAll("fieldset");
    let createFilledSurveyQuestions = [];
    for (let i = 0; i < surveyQuestions.length; i++) {
        let surveyCountId = surveyQuestions[i].id;
        let questionType = document.getElementById(`question-type-${surveyCountId}`).value;
        let question = document.getElementById(`question-${surveyCountId}`).value;
        let surveyQuestionOptions = [];
        if (parseInt(questionType) == 3) {
            let questionOptions = document.getElementsByClassName(`survey-question-options-${surveyCountId}`);
            for (let j = 0; j < questionOptions.length; j++) {
                let surveyQuestionOption = {
                    text: questionOptions[j].value
                };
                surveyQuestionOptions.push(surveyQuestionOption);
            }
        }
        let createFilledSurveyQuestion = {
            question: question,
            questionType: questionType,
            surveyQuestionOptions: surveyQuestionOptions
        };
        createFilledSurveyQuestions.push(createFilledSurveyQuestion)
    }
    let createFilledSurvey = {
        surveyQuestions: createFilledSurveyQuestions
    }
    $.ajax({
        type: "POST",
        url: "/survey/create-survey",
        data: {surveyViewModel: createFilledSurvey},
        async: false,
        success: function (response) {
            if (response.redirectToUrl!=null){
                window.location.href = response.redirectToUrl;
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function prepareSurveyAnswerFormData() {
    let checkInputsResult = checkInputs();
    if (checkInputsResult){
        alert("There are empty inputs!");
        return;
    }
    let surveyQuestions = document.querySelectorAll("fieldset");
    let createSurveyAnswerRequests = [];
    for (let i = 0; i < surveyQuestions.length; i++) {
        let surveyQuestionId = surveyQuestions[i].id;
        let questionType = parseInt(document.getElementById(`question-type-${surveyQuestionId}`).value);
        let createSurveyAnswerRequest = null;
        if (questionType == 0) {
            let surveyAnswer = document.getElementById(`question-${surveyQuestionId}`).value;
            createSurveyAnswerRequest = {
                surveyQuestionId: surveyQuestionId,
                singleLinePlainTextAnswer: surveyAnswer
            }
        } else if (questionType == 1) {
            let surveyAnswer = document.getElementById(`question-${surveyQuestionId}`).value;
            createSurveyAnswerRequest = {
                surveyQuestionId: surveyQuestionId,
                multipleLinePlainTextAnswer: surveyAnswer
            }
        } else if (questionType == 2) {
            let surveyAnswer = document.getElementById(`question-${surveyQuestionId}`).value;
            createSurveyAnswerRequest = {
                surveyQuestionId: surveyQuestionId,
                scoringAnswer: surveyAnswer
            }
        } else {
            let surveyAnswer = document.querySelector(`input[name="survey-question-option-${surveyQuestionId}"]:checked`).value;
            createSurveyAnswerRequest = {
                surveyQuestionId: surveyQuestionId,
                multipleChoiceAnswer: surveyAnswer
            }
        }
        createSurveyAnswerRequests.push(createSurveyAnswerRequest);
    }
    let createSurveyAnswersViewModel = {
        createSurveyAnswerRequests: createSurveyAnswerRequests
    }
    $.ajax({
        type: "POST",
        url: "/survey/create-survey-answers",
        data: {createSurveyAnswersViewModel: createSurveyAnswersViewModel},
        async: false,
        success: function (response) {
            window.location.href = response.redirectToUrl;
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function checkInputs() {
    let checkIfAnInputEmpty = false;

    $("input[type=text]").each(function () {
        let input = $(this);
        if (input.val() <= 0) {
            checkIfAnInputEmpty = true;
        } 
    });
    $("textarea").each(function () {
        let input = $(this);
        if (input.val() <= 0) {
            checkIfAnInputEmpty = true;
        }
    });
    $("input[type=radio]").each(function () {
        let anyRadioButtonChecked = false;
        let input = $(this);
        let name = input[0].getAttribute("name");
        let radioButtons = document.getElementsByName(name)
        for (let i=0;i<radioButtons.length;i++){
            if (radioButtons[i].checked==true){
                anyRadioButtonChecked = true
            }
        }
        if (!anyRadioButtonChecked){
            checkIfAnInputEmpty = true;
        }
    });
    
    return checkIfAnInputEmpty;
}
