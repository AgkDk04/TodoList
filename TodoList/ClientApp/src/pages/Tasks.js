/*eslint unicode-bom: ["error", "never"]*/
import React, { useEffect, useState } from 'react'
import axios from "axios";
import TodoRow from '../components/TodoRow';

function Tasks() {
    // Token header is already added in the utils
    const [Query, setQuery] = useState("");
    const [items,setItems] = useState([]);

    const handleQuery = (query) => { 
        setQuery(query.target.value);
    }

    useEffect(() => {
        getData();
    }, []);

    async function getData() {  
        axios.get("http://localhost:5126/TodoTask")
            .then(response => {
                setItems(response.data);
            })
            .catch(err => console.error(err));
    }

    const addTodoItem = () => {
        const config = {
            headers: { 'Content-Type': 'application/json' }
        };
        var item = {
            Text: Query,
            IsDone: false
        };
        axios.post("http://localhost:5126/TodoTask", JSON.stringify(item), config)
            .then(response => {
                getData();
                if (response.status >= 200) {
                    console.info("Added successfully!");
                    // alert('Added successfully!');
                }
            })
            .catch(err => console.error(err));
    }

    const changeTaskStatus = (id) => {
        const config = {
            params: { id: id }
        };
        axios.put("http://localhost:5126/TodoTask", null, config)
            .then(response => {
                getData();
                if (response.status >= 200) {
                    console.info("Good job! its marked as Done");
                }
            })
            .catch(err => console.error(err));
    }

    const removeTodoItem = (id) => {
        axios.delete(`http://localhost:5126/TodoTask/${id}`)
            .then(response => {
                getData();
                if (response.status >= 200) {
                    console.info("Deleted successfully!");
                }
            })
            .catch(err => console.error(err));
    }

    return (
            <div className="table-responsive-md">
                <h1>My todo tasks</h1>
                <div className="input-group mb-3 mt-4">
                    <input className="form-control mx-1" type="text" maxLength="50" placeholder="Type your todo text..." onChange={handleQuery} value={Query} />
                    <button type="button" className="btn btn-outline-primary mx-sm" onClick={addTodoItem}>ADD</button>
                </div>
                <div>
                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th scope="col">Text</th>
                                <th scope="col">Status</th>
                                <th scope="col">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map((item) => {
                                return <TodoRow key={item.id} color={"red"} itemText={item.Text} status={item.IsDone} id={item.Id}
                                    handleStatus={changeTaskStatus} remove={removeTodoItem} />;
                            })}
                        </tbody>
                    </table>
                </div>  
            </div>
    );
}

export default Tasks;