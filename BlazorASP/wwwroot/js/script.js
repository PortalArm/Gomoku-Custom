var dotnetHelper;
var disposed = false;
var initializeField = (size, dotnet) => {
    dotnetHelper = dotnet;
    disposed = false;
    console.log(`begin ${size}`);
    const field = document.getElementById("field");
    while(field.lastChild)
       field.lastChild.remove();

    for(let i = 0;i < size; ++i){
        var fieldrow = document.createElement("div");
        fieldrow.setAttribute("class", "game-field-row");
        for(let j = 0;j < size; ++j){
            var fieldcell = document.createElement("div");
            fieldcell.setAttribute("class", `game-field-cell`);
            fieldcell.setAttribute("data-row", i);
            fieldcell.setAttribute("data-col", j);
            fieldcell.addEventListener("click", processClick, { once: true });
            fieldrow.appendChild(fieldcell);
        }
        field.appendChild(fieldrow);
    }
    console.log(`end ${size}`);
};
var processClick = event => {
    const target = event.target;
    let row = parseInt(target.getAttribute("data-row"));
    let col = parseInt(target.getAttribute("data-col"));
    dotnetHelper.invokeMethodAsync('ReceivePos', row, col, teams[index]).then(r => console.log(r));
    //DotNet.invokeMethodAsync('Gomoku Custom.Blazor', 'SayHello', 'bruh').then(r => console.log(r));
    if(updateCell(row, col, teams[index])) index = (index + 1) % 2;
    //console.log(row + " " + col);
};
const teams = ["team-red", "team-blue"];
var index = 0;
var updateCell = (row, col, color) => {
    const obj = document.querySelector(`.game-field-cell[data-row='${row}'][data-col='${col}']`);
    if(!obj || obj.getAttribute("class") != 'game-field-cell')
        return false;
    obj.classList.add(color);
    return true;
};
document.addEventListener("beforeunload", e => {
    dotnetHelper.invokeMethodAsync('Dispose');
    disposed = true;
});
//document.onclose = e => {
//    alert("yep");
//    if (dotnetHelper !== null)
//        dotnetHelper.dispose();
//};


//document.addEventListener("DOMContentLoaded",e => initializeField(19));