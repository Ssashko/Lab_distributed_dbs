﻿using Lab_distributed_dbs.DAL.Settings;
using Lab_distributed_dbs.DAL;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Lab_distributed_dbs.Services
{
    public class StudentService
    {
        private readonly IMongoCollection<Student> _studentsCollection;
        public StudentService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient
        mongoClient)
        {
            var mongoDatabase =
            mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _studentsCollection = mongoDatabase.GetCollection<Student>("Students");
        }
        public async Task<List<Student>> GetAsync() =>
        await _studentsCollection.Find(s => true).ToListAsync();
        public async Task<Student> GetByIdAsync(int id) =>
        await _studentsCollection.Find(s => s.StudentId == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Student student) =>
        await _studentsCollection.InsertOneAsync(student);
        public async Task UpdateAsync(int id, Student updatedStudent) =>
        await _studentsCollection.ReplaceOneAsync(s => s.StudentId == id, updatedStudent);
        public async Task RemoveAsync(int id) =>
        await _studentsCollection.DeleteOneAsync(s => s.StudentId == id);
    }

    public class LecturerService
    {
        private readonly IMongoCollection<Lecturer> _lecturersCollection;
        public LecturerService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient
        mongoClient)
        {
            var mongoDatabase =
            mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _lecturersCollection = mongoDatabase.GetCollection<Lecturer>("Lecturers");
        }
        public async Task<List<Lecturer>> GetAsync() =>
        await _lecturersCollection.Find(l => true).ToListAsync();
        public async Task<Lecturer> GetByIdAsync(int id) =>
        await _lecturersCollection.Find(l => l.LecturerId == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Lecturer lecturer) =>
        await _lecturersCollection.InsertOneAsync(lecturer);
        public async Task UpdateAsync(int id, Lecturer updatedLecturer) =>
        await _lecturersCollection.ReplaceOneAsync(l => l.LecturerId == id, updatedLecturer);
        public async Task RemoveAsync(int id) =>
        await _lecturersCollection.DeleteOneAsync(l => l.LecturerId == id);
    }

}
