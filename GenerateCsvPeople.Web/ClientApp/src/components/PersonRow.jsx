const Row = ({ person }) => {


    return (
        <tr>
            <td>{person.id}</td>
            <td>{person.firstName}</td>
            <td>{person.lastName}</td>
            <td>{person.age}</td>
            <td>{person.address}</td>
            <td>{person.email}</td>
        </tr>
    )
}

export default Row