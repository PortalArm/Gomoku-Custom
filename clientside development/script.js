var initializeField = (size) => {
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
            fieldcell.addEventListener("click", processClick, { once: true});
            fieldrow.appendChild(fieldcell);
        }
        field.appendChild(fieldrow);
    }
    console.log(`end ${size}`);
};
var processClick = event => {
    const target = event.target;
    let row = target.getAttribute("data-row");
    let col = target.getAttribute("data-col");
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
//document.addEventListener("DOMContentLoaded",e => initializeField(19));