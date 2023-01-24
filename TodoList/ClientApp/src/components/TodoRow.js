import React from 'react'

function TodoRow({ itemText, status, id, handleStatus, remove }) {
    return (
        <tr>
            <td className="align-middle">{itemText}</td>
            <td className="align-middle">{status ? "Done" : "Not done"}</td>
            <td className="align-bottom">
                <p><button type="button" disabled={status} className="btn btn-success m-1" onClick={() => handleStatus(id)}>{status ? "Good job!" : "Mark as Done!"} </button>
                <button type="button" className="btn btn-danger" onClick={() => remove(id)}>Delete</button></p></td>
        </tr>
    )
}

export default TodoRow