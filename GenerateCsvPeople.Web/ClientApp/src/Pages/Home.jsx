import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Home.css';
import axios from 'axios';
import Row from '../components/PersonRow';

const Home = () => {

    const [people, setPeople] = useState([])

    const getPeople = async () => {
        const { data } = await axios.get('/api/people/getall')
        setPeople(data)
    }

    useEffect(() => {
        

        getPeople()
    }, [])

    const onDeleteClick = async () => {
        await axios.post('/api/people/delete')
        getPeople()
    }

    return (
        <div className="container" style={{ marginTop: '60px' }}>
            <div className="row">
                <div className="col-md-6 offset-md-3 mt-5">
                    <button className="btn btn-danger btn-lg w-100" onClick={onDeleteClick}>Delete All</button>
                </div>
            </div>

            <table className="table table-hover table-striped table-bordered mt-5">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Age</th>
                        <th>Address</th>
                        <th>Email</th>
                    </tr>
                </thead>
                <tbody>
                    {people.map(p => <Row key={p.id} person={p} />)}
                </tbody>
            </table>
        </div>
    );
};

export default Home;