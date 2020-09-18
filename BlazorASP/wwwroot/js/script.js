var dotnetHelper;
//var disposed = false;
var index = 1;
const teams = ["", "team-blue", "team-red"];
var isMyTurn = async () => {
    let result = await dotnetHelper.invokeMethodAsync('IsClientTurn');
    return result;
};
var initializeField = (size, dotnet, extfield) => {
    dotnetHelper = dotnet;
    const field = document.getElementById("field");
    while(field.lastChild)
       field.lastChild.remove();

    for(let i = 0;i < size; ++i){
        var fieldrow = document.createElement("div");
        fieldrow.setAttribute("class", "game-field-row");
        for(let j = 0;j < size; ++j){
            var fieldcell = document.createElement("div");
            fieldcell.setAttribute("class", "game-field-cell");
            fieldcell.setAttribute("data-row", i);
            fieldcell.setAttribute("data-col", j);
            fieldcell.addEventListener("click", processClick, { once: true });
            fieldrow.appendChild(fieldcell);
        }
        field.appendChild(fieldrow);
    }
    if(extfield)
        populateField(extfield);
    
};
var populateField = field => {
    for (let i = 0; i < field.length; ++i)
        for (let j = 0; j < field[i].length; ++j)
            if(field[i][j] != 0)
               updateCell(i,j,field[i][j]);
};
var processClick = async event => {
    if (!await isMyTurn()) {
        console.log("It's not your turn!");
        event.target.addEventListener("click", processClick, { once: true });
        return;
    }
    const target = event.target;
    let row = parseInt(target.getAttribute("data-row"));
    let col = parseInt(target.getAttribute("data-col"));
    await dotnetHelper.invokeMethodAsync('ReceivePos', row, col);
};
var updateCell = (row, col, color) => {
    const obj = document.querySelector(`.game-field-cell[data-row='${row}'][data-col='${col}']`);
    if (!obj || obj.getAttribute("class") != 'game-field-cell' || color === "")
        return false;
    obj.removeEventListener("click", processClick);
    obj.classList.add(teams[color]);
    return true;
};
